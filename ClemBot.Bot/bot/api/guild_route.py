import typing as t

import discord

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

    async def get_guild_user_ids(self, guild_id: int):
        guild = await self.client.get(f'guilds/{guild_id}')

        if guild.status != 200:
            return

        return guild.value['users']


    async def edit_guild(self, guild_id: int, name: str):
        json = {
            'id': guild_id,
            'name': name,
        }

        await self.client.patch('guilds/edit', data=json)

    async def update_guild_users(self, guild_id: int, name: str, users: t.List[t.Dict[t.Any, t.Any]]):
        json = {
            'name': name,
            'users': users
        }

        await self.client.patch(f'guilds/{guild_id}/update/users', data=json)

    async def update_guild_roles(self, guild_id: int, roles: t.List[discord.Role]):
        roles = [{
                'id': r.id,
                'name': r.name
            }
            for r in roles]

        json = {
            'roles': roles
        }

        await self.client.patch(f'guilds/{guild_id}/update/roles', data=json)

    async def update_guild_channels(self, guild_id: int, channels: t.List[discord.TextChannel]):
        channels = [{
            'id': r.id,
            'name': r.name
        }
            for r in channels]

        json = {
            'channels': channels
        }

        await self.client.patch(f'guilds/{guild_id}/update/channels', data=json)
