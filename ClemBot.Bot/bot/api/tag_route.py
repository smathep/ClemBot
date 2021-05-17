import typing as t

import discord

from api.api_client import ApiClient
from api.base_route import BaseRoute


class TagRoute(BaseRoute):

    def __init__(self, api_client: ApiClient):
        super().__init__(api_client)

    async def create_tag(self, name: str, content: str, guild_id: int, user_id: int):
        json = {
            'Name': name,
            'Content': content,
            'GuildId': guild_id,
            'UserId': user_id,
        }
        await self.client.post('tags', data=json)

    async def edit_tag(self, guild_id: int, name: str, content: str):
        json = {
            'GuildId': guild_id,
            'Name': name,
            'Content': content
        }
        await self.client.patch('tags', data=json)

    async def get_tag(self, guild_id: int, name: str):
        json = {
            'GuildId': guild_id,
            'Name': name,
        }
        return (await self.client.get('tags', data=json)).value

    async def get_tag_content(self, guild_id: int, name: str):
        json = {
            'GuildId': guild_id,
            'Name': name,
        }
        return (await self.client.get('tags', data=json)).value['content']

    async def delete_tag(self, guild_id: int, name: str):
        json = {
            'GuildId': guild_id,
            'Name': name,
        }
        await self.client.delete('tags', data=json)

    async def add_tag_use(self, guild_id: int, name: str, channel_id: int, user_id: int):
        json = {
            'GuildId': guild_id,
            'Name': name,
            'ChannelId': channel_id,
            'UserId': user_id
        }

        await self.client.post('tags/invoke', data=json)

    async def get_guilds_tags(self, guild_id: int) -> t.Optional[t.List[int]]:
        users = await self.client.get(f'guilds/{guild_id}/tags')

        if users.status != 200:
            return

        return users.value
