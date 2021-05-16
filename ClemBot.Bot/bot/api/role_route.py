import typing as t

from api.api_client import ApiClient
from api.base_route import BaseRoute


class RoleRoute(BaseRoute):

    def __init__(self, api_client: ApiClient):
        super().__init__(api_client)

    async def create_role(self, role_id: int, name: int, guild_id: int):
        json = {
            'id': role_id,
            'name': name,
            'guildId': guild_id
        }
        await self.client.post('roles', data=json)

    async def get_role(self, role_id: int):
        user = await self.client.get(f'roles/{role_id}')

        if user.status != 200:
            return

        return user.value

    async def edit_role(self, role_id: int, name: str):
        json = {
            'id': role_id,
            'name': name,
        }

        await self.client.patch('roles', data=json)

    async def remove_role(self, role_id: int):

        await self.client.delete(f'roles/{role_id}')

    async def get_guilds_roles(self, guild_id: int) -> t.Optional[t.List[int]]:
        users = await self.client.get(f'guilds/{guild_id}/roles')

        if users.status != 200:
            return

        return users.value
