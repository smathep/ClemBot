import typing as t

from api.api_client import ApiClient
from api.base_route import BaseRoute


class UserRoute(BaseRoute):

    def __init__(self, api_client: ApiClient):
        super().__init__(api_client)

    async def create_user(self, user_id: int, name: str):
        json = {
            'id': user_id,
            'name': name,
        }
        await self.client.post('users', data=json)

    async def add_user_guild(self, user_id: int, guild_id: int):
        json = {
            'GuildId': guild_id,
            'UserId': user_id,
        }
        await self.client.post('guilds/adduser', data=json)

    async def remove_user_guild(self, user_id: int, guild_id: int):
        json = {
            'GuildId': guild_id,
            'UserId': user_id,
        }
        await self.client.delete('guilds/removeuser', data=json)

    async def get_user_guilds_ids(self, user_id: int):
        user = await self.client.get(f'users/{user_id}')

        if user.status != 200:
            return

        return [g.id for g in user.value['Guilds']]

    async def get_users_ids(self) -> t.Optional[t.List[int]]:
        users = await self.client.get(f'users')

        if users.status != 200:
            return

        return [u['id'] for u in users.value]

