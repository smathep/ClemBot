import json

from api.api_client import ApiClient
from api.base_route import BaseRoute


class GuildRoute(BaseRoute):

    def __init__(self, api_client: ApiClient):
        super().__init__(api_client)

    async def add_guild(self, guild_id: int, name: str):
        if len((await self.client.get(f'guilds/{guild_id}')).value) > 0:
            return

        guild = {
           'id': guild_id,
           'name': name
        }
        await self.client.post('guilds', data=json.dumps(guild))


