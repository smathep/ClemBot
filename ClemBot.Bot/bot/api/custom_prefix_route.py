import typing as t

import discord

from api.api_client import ApiClient
from api.base_route import BaseRoute


class CustomPrefixRoute(BaseRoute):

    def __init__(self, api_client: ApiClient):
        super().__init__(api_client)

    async def set_custom_prefix(self, guild_id: int, prefix: str):
        json = {
            'GuildId': guild_id,
            'Prefix': prefix
        }
        return await self.client.post('customprefixes/add', data=json)

    async def remove_custom_prefix(self, guild_id: int, prefix: str):
        json = {
            'GuildId': guild_id,
            'Prefix': prefix
        }
        return await self.client.delete('customprefixes/remove', data=json)

    async def get_custom_prefixes(self, guild_id: int):
        resp = await self.client.get(f'guilds/{guild_id}/customprefixes')
        return resp.value

