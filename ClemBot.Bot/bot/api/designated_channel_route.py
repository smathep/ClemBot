from api.api_client import ApiClient
from api.base_route import BaseRoute


class DesignatedChannelRoute(BaseRoute):

    def __init__(self, api_client: ApiClient):
        super().__init__(api_client)

    async def register_channel(self,
                               channel_id: int,
                               designation: str):
        json = {
            'ChannelId': channel_id,
            'Designation': designation
        }

        return await self.client.post('designatedchannels', data=json)

    async def delete_channel(self,
                             channel_id: int,
                             designation: str):
        json = {
            'ChannelId': channel_id,
            'Designation': designation
        }

        return await self.client.delete('designatedchannels', data=json)

    async def get_guild_designated_channel_ids(self,
                                               guild_id: int,
                                               designation: str):
        json = {
            'GuildId': guild_id,
            'Designation': designation
        }
        resp = await self.client.get('designatedchannels/details', data=json)

        if resp.status != 200:
            return

        return resp.value['mappings']

    async def get_guild_all_designated_channels(self, guild_id: int, ):
        resp = await self.client.get(f'guild/{guild_id}/designatedchannels')

        if resp.status != 200:
            return

        return {i['designation']: i['channelIds'] for i in resp.value}

    async def get_global_designations(self, designation: str, ):
        resp = await self.client.get(f'designatedchannels/{designation}/index')

        if resp.status != 200:
            return

        return resp.value
