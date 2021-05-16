import typing as t

from api.api_client import ApiClient
from api.base_route import BaseRoute


class ChannelRoute(BaseRoute):

    def __init__(self, api_client: ApiClient):
        super().__init__(api_client)

    async def create_channel(self, channel_id: int, name: int, guild_id: int):
        json = {
            'id': channel_id,
            'name': name,
            'guildId': guild_id
        }
        await self.client.post('channels', data=json)

    async def get_channel(self, channel_id: int):
        user = await self.client.get(f'channels/{channel_id}')

        if user.status != 200:
            return

        return user.value

    async def edit_channel(self, channel_id: int, name: str):
        json = {
            'id': channel_id,
            'name': name,
        }

        await self.client.patch('channels', data=json)

    async def remove_channel(self, channel_id: int):

        await self.client.delete(f'channels/{channel_id}')

    async def get_guilds_channels(self, guild_id: int) -> t.Optional[t.List[int]]:
        users = await self.client.get(f'guilds/{guild_id}/channels')

        if users.status != 200:
            return

        return users.value
