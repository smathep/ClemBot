import json
import logging
import typing as t
from urllib.parse import quote

import aiohttp

import bot.bot_secrets as bot_secrets
from bot.consts import Urls
from bot.errors import ApiClientRequestError, BotOnlyRequestError

log = logging.getLogger(__name__)


class Result:
    def __init__(self, status: int, value: t.Any):
        self.status = status
        self.value = value

    def __str__(self):
        return f'Result Status: {self.status}\nValue:\n{json.dumps(self.value, indent=2)}'


class HttpRequestType:
    get = 'GET'
    put = 'PUT'
    post = 'POST'
    delete = 'DELETE'
    patch = 'PATCH'


class ApiClient:

    def __init__(self, *, reconnect_callback=None, bot_only: bool = False):
        self.auth_token: str = None
        self.session: aiohttp.ClientSession = None
        self.connected: bool = False

        self.bot_only = bot_only

        # Create an empty async method so our callback doesnt throw when we await it
        async def async_stub():
            pass

        self.reconnect_callback = reconnect_callback or async_stub

    @staticmethod
    def _url_for(url: str):
        url = f'{bot_secrets.secrets.api_url}{Urls.base_api_url}{quote(url)}'
        log.info(f'Building URL: {url}')
        return url

    async def _get_auth_token(self):
        async with self.session.request(HttpRequestType.get,
                                        self._url_for('authorize'),
                                        ssl=False,
                                        params={'key': bot_secrets.secrets.api_key}
                                        ) as resp:
            if resp.status != 200:
                self.connected = False
                log.error(f'Connecting to ClemBot.Api at Url: {bot_secrets.secrets.api_url} '
                          f'failed with response code: {resp.status}')
                return

            self.connected = True
            resp_json = await resp.json()
            return resp_json['token']

    async def _authorize(self):

        if self.bot_only:
            log.info('Bot in bot_only mode, skipping Api authorization')
            return

        headers = {
            'Accept': '*/*'
        }
        log.info('Requesting ClemBot.Api Access token')

        # Check if we have an active session, this means we are trying to reconnect
        # if we are close the session and create a new one
        if self.session:
            await self.session.close()
        self.session = aiohttp.ClientSession(headers=headers)

        self.auth_token = await self._get_auth_token()

        if not self.connected:
            raise ConnectionError('Connecting to ClemBot.Api Failed')

        log.info('Connecting to ClemBot.Api Successful')

        await self.session.close()

        headers = {
            'Authorization': f'BEARER {self.auth_token}',
            'Content-type': 'application/json',
            'Accept': 'application/json'
        }
        log.info('Initialized JWT BEARER token Auth Headers')

        self.session = aiohttp.ClientSession(headers=headers)

    async def connect(self):
        log.info(f'Connecting to ClemBot.Api at URL: {bot_secrets.secrets.api_url}')
        await self._authorize()

    async def _reconnect(self):
        log.info(f'Attempting to reconnect to ClemBot.Api at URL: {bot_secrets.secrets.api_url}')
        await self._authorize()

        if self.connected:
            log.info('Successfully reconnected to ClemBot.Api')
            await self.reconnect_callback()

    async def close(self) -> None:
        """Close the aiohttp session."""
        await self.session.close()

    async def _request(self, http_type: str, endpoint, **kwargs):

        if self.bot_only:
            raise BotOnlyRequestError("Request Failed: Bot is in bot_only mode")

        log.info(f'HTTP {http_type} Request initializing to route: {endpoint}')
        if 'data' in kwargs:
            data = json.dumps(kwargs['data'], indent=2)
            log.info(f'HTTP {http_type} data found {data}')
            kwargs['data'] = data

        async with self.session.request(http_type,
                                        self._url_for(endpoint),
                                        ssl=False,
                                        **kwargs
                                        ) as resp:

            if resp.status != 200:
                log.warning(f'HTTP Request at endpoint {endpoint} returned {resp.status} status')
                return Result(resp.status, None)

            res = Result(resp.status, await resp.json())

        log.info(f'Result for HTTP {http_type} at endpoint: {endpoint}\n{res}')
        return res

    async def _request_or_reconnect(self, http_type: str, endpoint, **kwargs):

        raise_on_error = kwargs.pop('raise_on_error', False)

        try:
            resp = await self._request(http_type, endpoint, **kwargs)
        except aiohttp.client_exceptions.ClientConnectorError as e:
            log.error(f'Request to ClemBot.Api Failed: No server found')
            raise e

        if resp.status == 200:
            return resp

        if resp.status == 401:
            self.connected = False
            log.warning(f'Request at endpoint: {endpoint} Failed with status code {resp.status}. Attempting to Reconnect to API...')
            await self._reconnect()

            if not self.connected:
                raise ConnectionError('Reconnecting to ClemBot.Api Failed')

            log.info(f'Retrying failed request at endpoint: {endpoint}')
            return await self._request(http_type, endpoint, **kwargs)

        if raise_on_error:
            raise ApiClientRequestError(f'Request at endpoint: {endpoint} Failed with status code {resp.status}')

        return resp

    async def get(self, endpoint: str, **kwargs):
        return await self._request_or_reconnect(HttpRequestType.get, endpoint, **kwargs)

    async def post(self, endpoint: str, **kwargs):
        return await self._request_or_reconnect(HttpRequestType.post, endpoint, **kwargs)

    async def patch(self, endpoint: str, **kwargs):
        return await self._request_or_reconnect(HttpRequestType.patch, endpoint, **kwargs)

    async def put(self, endpoint: str, **kwargs):
        return await self._request_or_reconnect(HttpRequestType.put, endpoint, **kwargs)

    async def delete(self, endpoint: str, **kwargs):
        return await self._request_or_reconnect(HttpRequestType.delete, endpoint, **kwargs)
