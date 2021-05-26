import typing as t

import discord

from api.api_client import ApiClient
from api.base_route import BaseRoute


class WelcomeMessageRoute(BaseRoute):

    def __init__(self, api_client: ApiClient):
        super().__init__(api_client)

    async def set_welcome_message(self, guild_id: int, message: str):

        json = {
            'Message': message
        }
        return await self.client.post(f'guilds/{guild_id}/SetWelcomeMessage', data=json)

    async def get_welcome_message(self, guild_id: int):
        resp = await self.client.get(f'guilds/{guild_id}/GetWelcomeMessage')
        return resp.value

    async def delete_welcome_message(self, guild_id: int):
        resp = await self.client.delete(f'guilds/{guild_id}/GetWelcomeMessage')
        return resp.value
