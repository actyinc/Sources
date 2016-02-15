package com.example.acty.droid3.DBObjects;

public class Campaign extends CampaignBase {
    private CampaignMedia campaignVisualResource;

    public CampaignMedia getCampaignVisualResource() {
        return campaignVisualResource;
    }

    public void setCampaignVisualResource(CampaignMedia campaignVisualResource) {
        this.campaignVisualResource = campaignVisualResource;
    }
}

