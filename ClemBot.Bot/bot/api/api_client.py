import dataclasses
import json
import logging
import typing as t
from urllib.parse import quote

import aiohttp

import bot_secrets
from consts import Urls

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

    def __init__(self):
        headers = {'Content-type': 'application/json', 'Accept': 'application/json'}
        self.session = aiohttp.ClientSession(headers=headers)

    @staticmethod
    def _url_for(url: str):
        url = f'{bot_secrets.secrets.api_url}{Urls.base_api_url}{quote(url)}'
        log.info(f'Building URL: {url}')
        return url

    async def close(self) -> None:
        """Close the aiohttp session."""
        await self.session.close()

    async def _request(self, http_type: str, endpoint, **kwargs):
        log.info(f'HTTP {http_type} Request initializing to route: {endpoint}')
        if 'data' in kwargs:
            data = json.dumps(kwargs['data'], indent=2)
            log.info(f'HTTP {http_type} data found:\n{data}')
            kwargs['data'] = data

        async with self.session.request(http_type,
                                        self._url_for(endpoint),
                                        ssl=False,
                                        **kwargs
                                        ) as resp:
            if resp.status != 200:
                log.info('HTTP Request returned non 200 status')
                return Result(resp.status, None)

            res = Result(resp.status, await resp.json())
            log.info(f'Result for HTTP {http_type} at endpoint: {endpoint}\n{res}')
            return res

    async def get(self, endpoint: str, **kwargs):
        return await self._request(HttpRequestType.get, endpoint, **kwargs)

    async def post(self, endpoint: str, **kwargs):
        return await self._request(HttpRequestType.post, endpoint, **kwargs)

    async def patch(self, endpoint: str, **kwargs):
        return await self._request(HttpRequestType.patch, endpoint, **kwargs)

    async def put(self, endpoint: str, **kwargs):
        return await self._request(HttpRequestType.put, endpoint, **kwargs)

    async def delete(self, endpoint: str, **kwargs):
        return await self._request(HttpRequestType.delete, endpoint, **kwargs)
