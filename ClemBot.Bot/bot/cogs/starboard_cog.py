# import the logging framework to allow us to log internally
# what the bot does
import logging
import discord
import discord.ext.commands as commands
from bot.clem_bot import ClemBot
import bot.extensions as ext
from bot.services.starboard_service import StarboardService
# get a module level logger using the __name__ of the module as the root,
# this will link it with the base logger bot. and all out put will be through that
log = logging.getLogger(__name__)


# We create a class with the postfix of "Cog"
# and make sure it inherits from Commands.cog
class StarboardCog(commands.Cog):
    """None"""

    # entry point of the cog, the d.py library supplies us with the bot
    # parameter which is the client object of the running bot
    # its with this that we can access all parts of the discord api
    def __init__(self, bot: ClemBot):
        self.bot = bot

    @ext.group(invoke_without_command=True, case_insensitive=True)
    @ext.long_help('Returns the current number of stars needed for a post to make it to starboard')
    @ext.short_help('Returns starboard threshold')
    @ext.example('starboard')
    async def starboard(self, ctx: commands.Context):
        """Returns the current number of stars needed for a post to make it to starboard"""
        threshold = await StarboardService.get_starboard_threshold(self, message=ctx.message)
        return await ctx.send(f'Minimum number of stars: {threshold}')

    @starboard.command()
    @ext.long_help('Sets the number of stars needed for a post to make it to starboard')
    @ext.short_help('Sets starboard threshold')
    @ext.example('starboard set <threshold>')
    async def set(self, ctx: commands.Context, threshold: int):
        # await self.bot.custom_starboard_threshold_route.set_custom_starboard_threshold(ctx.guild.id, threshold)
        response = await self.bot.guild_route.set_starboard_threshold(ctx.guild.id, threshold)
        return await ctx.send(f'Starboard threshold set to { await self.bot.guild_route.get_starboard_threshold(ctx.guild.id)}')


def setup(bot):
    bot.add_cog(StarboardCog(bot))
