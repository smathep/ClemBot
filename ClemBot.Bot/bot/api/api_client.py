import dataclasses
from urllib.parse import quote
import typing as t
import logging

import aiohttp

from consts import Urls
import bot_secrets

log = logging.getLogger(__name__)


@dataclasses.dataclass()
class Result:
    status: int
    value: t.Any


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

        async with self.session.request(http_type, self._url_for(endpoint), ssl=False, **kwargs) as resp:
            return Result(resp.status, await resp.json())

    async def get(self, endpoint: str, **kwargs):
        return await self._request('GET', endpoint, **kwargs)

    async def post(self, endpoint: str, **kwargs):
        return await self._request('POST', endpoint, **kwargs)

    async def patch(self, endpoint: str, **kwargs):
        return await self._request('PATCH', endpoint, **kwargs)

    async def put(self, endpoint: str, **kwargs):
        return await self._request('PUT', endpoint, **kwargs)
