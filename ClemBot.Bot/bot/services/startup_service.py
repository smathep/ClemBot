import asyncio
import logging

import discord

from bot.services.base_service import BaseService

log = logging.getLogger(__name__)


class StartupService(BaseService):
    """
    Service to reload discord state into the database on restart
    this is to account for any leaves or joins, new roles, new channels etc
    that happened while the bot was offline
    """

    def __init__(self, *, bot):
        super().__init__(bot)

    async def load_guilds(self):
        tasks = []
        for guild in self.bot.guilds:
            if not await self.bot.guild_route.get_guild(guild.id):
                log.info(f'Loading guild {guild.name}: {guild.id}')
                tasks.append(asyncio.create_task(self.bot.guild_route.add_guild(guild.id, guild.name)))
        await asyncio.gather(*tasks)

    async def load_users(self):
        db_users = await self.bot.user_route.get_users_ids()
        new_users = [u for u in self.bot.users if u.id not in db_users]
        tasks = []
        for user in new_users:
            log.info('hi')
            tasks.append(await self.bot.user_route.create_user(user.id, user.name))
        await asyncio.gather(*tasks)

    async def load_users_guilds(self):
        tasks = []
        for guild in self.bot.guilds:
            user_ids = [u.id for u in guild.members]
            tasks.append(asyncio.create_task(self.bot.guild_route.update_guild(guild.id, guild.name, user_ids)))

        await asyncio.gather(*tasks)

    def get_full_name(self, author) -> str:
        return f'{author.name}#{author.discriminator}'

    async def load_service(self):
        # First load any new guilds so that we can reference them
        await self.load_guilds()

        # Load new users, this will pull known users and compare to current users and only add the new ones
        await self.load_users()

        # Load user guild relationships, takes every guild and sends a complete list of users to the backend
        # to replace the current known state
        await self.load_users_guilds()


