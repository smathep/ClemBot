import logging

import discord

from bot.messaging.events import Events
from bot.services.base_service import BaseService

log = logging.getLogger(__name__)


class ChannelHandlingService(BaseService):

    def __init__(self, *, bot):
        super().__init__(bot)

    @BaseService.Listener(Events.on_guild_channel_create)
    async def channel_create(self, channel):
        log.info(f'New channel created {channel.name}:{channel.id} in guild: {channel.guild.name}:{channel.guild.id}')
        await self.bot.channel_route.create_channel(channel.id, channel.name, channel.guild.id)

    @BaseService.Listener(Events.on_guild_channel_delete)
    async def channel_delete(self, channel):
        log.info(f'Channel deleted {channel.name}:{channel.id} in guild: {channel.guild.name}:{channel.guild.id}')
        await self.bot.channel_route.remove_channel(channel.id)

    @BaseService.Listener(Events.on_guild_channel_update)
    async def channel_update(self, before, after):
        await self.bot.channel_route.edit_channel(after.id, after.name)

    async def load_service(self):
        pass
