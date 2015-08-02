package com.example.acty.droid3;

public class DBCampaign extends CampaignBase {
    private String campaignMediaResourceBlob;
    private String campaignId;


    public String getCampaignMediaResourceBlob() {
        return campaignMediaResourceBlob;
    }

    public void setCampaignMediaResourceBlob(String campaignMediaResourceBlob) {
        this.campaignMediaResourceBlob = campaignMediaResourceBlob;
    }

    public String getCampaignId() {
        return campaignId;
    }

    public void setCampaignId(String campaignId) {
        this.campaignId = campaignId;
    }
}

