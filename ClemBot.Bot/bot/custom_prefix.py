import logging

import discord
from discord.ext import commands

from bot.clem_bot import ClemBot

log = logging.getLogger(__name__)


class CustomPrefix:

    def __init__(self, *, default: str):
        log.info(f'Setting default prefix too: "{default}""')
        self.default = default

    async def get_prefix(self, bot: ClemBot, message: discord.Message):
        prefixes = await bot.custom_prefix_route.get_custom_prefixes(message.guild.id)

        if len(prefixes) == 0:
            prefixes = [self.default]

        return commands.when_mentioned(bot, message) + prefixes
