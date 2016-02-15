package com.example.acty.droid3.DBObjects;

public class DBCampaign extends CampaignBase {
    private String campaignMediaResourceBlob;
    private String campaignId;
    private String contentName;
    private String contentType;

    public String getCampaignMediaResourceBlob() {
        return this.campaignMediaResourceBlob;
    }

    public void setCampaignMediaResourceBlob(String campaignMediaResourceBlob) {
        this.campaignMediaResourceBlob = campaignMediaResourceBlob;
    }

    public String getCampaignId() {
        return this.campaignId;
    }

    public void setCampaignId(String campaignId) {
        this.campaignId = campaignId;
    }

    public String getContentName() {
        return this.contentName;
    }

    public void setContentName(String contentName) {
        this.contentName = contentName;
    }

    public String getContentType() {
        return this.contentType;
    }

    public void setContentType(String contentType) {
        this.contentType = contentType;
    }
}

