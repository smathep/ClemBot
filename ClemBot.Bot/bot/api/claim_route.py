import discord

from bot.api.api_client import ApiClient
from bot.api.base_route import BaseRoute
from bot.consts import Claims


class ClaimRoute(BaseRoute):

    def __init__(self, api_client: ApiClient):
        super().__init__(api_client)

    async def add_claim_mapping(self, claim: Claims, role_id: int):
        json = {
            'RoleId': role_id,
            'Claim': claim
        }

        await self._client.post('claimmappings', data=json)

    async def remove_claim_mapping(self, claim: Claims, role_id: int):
        json = {
            'RoleId': role_id,
            'Claim': claim
        }

        await self._client.delete('claimmappings', data=json)

    async def get_claims_role(self, role_id):
        resp = await self._client.get(f'roles/{role_id}/claimmappings')

        if resp.status != 200:
            return None

        return resp.value

    async def get_claims_user(self, user: discord.Member):
        claims = set()
        for role_id in user.roles:
            resp = await self._client.get(f'roles/{role_id}/claimmappings')

            if resp.status != 200:
                return None

            for claim in resp.value:
                claims.add(claim)

        return claims

    async def check_claim_role(self, claim: Claims, role: discord.Role) -> bool:
        return claim in await self.get_claims_role(role.id)

    async def check_claim_user(self, claim: Claims, user: discord.Member) -> bool:
        return claim in await self.get_claims_user(user)
