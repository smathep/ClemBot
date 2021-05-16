import logging

import discord

from bot.data.channel_repository import ChannelRepository
from bot.messaging.events import Events
from bot.services.base_service import BaseService

log = logging.getLogger(__name__)


class ChannelHandlingService(BaseService):

    def __init__(self, *, bot):
        super().__init__(bot)

    @BaseService.Listener(Events.on_guild_channel_create)
    async def channel_create(self, channel):
        repo = ChannelRepository()
        log.info(f'New channel created {channel.name}:{channel.id} in guild: {channel.guild.name}:{channel.guild.id}')
        await repo.add_channel(channel)
        await self.bot.channel_route.create_channel(channel.id, channel.name, channel.guild.id)

    @BaseService.Listener(Events.on_guild_channel_delete)
    async def channel_delete(self, channel):
        repo = ChannelRepository()
        log.info(f'Channel deleted {channel.name}:{channel.id} in guild: {channel.guild.name}:{channel.guild.id}')
        await repo.delete_channel(channel)
        await self.bot.channel_route.remove_channel(channel.id)

    @BaseService.Listener(Events.on_guild_channel_update)
    async def channel_update(self, before, after):
        repo = ChannelRepository()
        await repo.update_channel(after)
        await self.bot.channel_route.edit_channel(after.id, after.name)

    @BaseService.Listener(Events.on_new_guild_initialized)
    async def new_guild_joined(self, guild: discord.Guild):
        repo = ChannelRepository()
        for c in guild.channels:
            log.info(f'Loading new channel: {c.name}:{c.id} in guild: {guild.name}:{guild.id}')
            await repo.add_channel(c)

    async def load_service(self):
        repo = ChannelRepository()
        for guild in self.bot.guilds:
            for channel in guild.channels:
                await repo.add_channel(channel)
