import typing as t

from api.api_client import ApiClient
from api.base_route import BaseRoute


class GuildRoute(BaseRoute):

    def __init__(self, api_client: ApiClient):
        super().__init__(api_client)

    async def add_guild(self, guild_id: int, name: str):
        if (await self.client.get(f'guilds/{guild_id}')).value:
            return

        json = {
            'id': guild_id,
            'name': name
        }
        await self.client.post('guilds', data=json)

    async def get_all_guilds_ids(self):
        guilds = await self.client.get('guilds')

        if guilds.status != 200:
            return

        return [g['id'] for g in guilds.value]

    async def get_guild(self, guild_id: int):
        guild = await self.client.get(f'guilds/{guild_id}')

        if guild.status != 200:
            return

        return guild.value

    async def update_guild(self, guild_id: int, name: str, users: t.List[int]):
        json = {
            'id': guild_id,
            'name': name,
            'users': users
        }

        await self.client.patch('guilds/update', data=json)
