import typing as t

from bot.api.api_client import ApiClient
from bot.api.base_route import BaseRoute


class RoleRoute(BaseRoute):

    def __init__(self, api_client: ApiClient):
        super().__init__(api_client)

    async def create_role(self, role_id: int, name: int, is_admin: bool, guild_id: int):
        json = {
            'id': role_id,
            'name': name,
            'admin': is_admin,
            'guildId': guild_id
        }
        await self._client.post('roles', data=json)

    async def get_role(self, role_id: int):
        user = await self._client.get(f'roles/{role_id}')

        if user.status != 200:
            return

        return user.value

    async def edit_role(self, role_id: int, name: str, is_admin: bool):
        json = {
            'id': role_id,
            'name': name,
            'admin': is_admin
        }

        await self._client.patch('roles', data=json)

    async def remove_role(self, role_id: int):
        await self._client.delete(f'roles/{role_id}')

    async def get_guilds_roles(self, guild_id: int) -> t.Optional[t.List[int]]:
        users = await self._client.get(f'guilds/{guild_id}/roles')

        if users.status != 200:
            return

        return users.value
