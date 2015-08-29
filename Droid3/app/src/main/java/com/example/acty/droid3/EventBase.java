package com.example.acty.droid3;

import java.util.List;

/**
 * Created by Hemanth on 7/26/2015.
 */
public class EventBase {
    private String CreatedDate;
    private String LastUpdatedDate;
    private String OwnerId;
    private String CampaignId;
    private List<String> Followers;
    private List<String> Comments;
    private String Description;
    private String Time;
    private String ZipCode;
    private String Country;
    private String[] KeyWords;
    private String Location;

    // This could be something like link to google/outlook calender event.
    // Google hangout link.
    private String ExternalEventLink;

    public String getCreatedDate() {
        return CreatedDate;
    }

    public void setCreatedDate(String createdDate) {
        CreatedDate = createdDate;
    }

    public String getLastUpdatedDate() {
        return LastUpdatedDate;
    }

    public void setLastUpdatedDate(String lastUpdatedDate) {
        LastUpdatedDate = lastUpdatedDate;
    }

    public String getOwnerId() {
        return OwnerId;
    }

    public void setOwnerId(String ownerId) {
        OwnerId = ownerId;
    }

    public String getCampaignId() {
        return CampaignId;
    }

    public void setCampaignId(String campaignId) {
        CampaignId = campaignId;
    }

    public List<String> getFollowers() {
        return Followers;
    }

    public void setFollowers(List<String> followers) {
        Followers = followers;
    }

    public List<String> getComments() {
        return Comments;
    }

    public void setComments(List<String> comments) {
        Comments = comments;
    }

    public String getDescription() {
        return Description;
    }

    public void setDescription(String description) {
        Description = description;
    }

    public String getTime() {
        return Time;
    }

    public void setTime(String time) {
        Time = time;
    }

    public String getZipCode() {
        return ZipCode;
    }

    public void setZipCode(String zipCode) {
        ZipCode = zipCode;
    }

    public String getCountry() {
        return Country;
    }

    public void setCountry(String country) {
        Country = country;
    }

    public String[] getKeyWords() {
        return KeyWords;
    }

    public void setKeyWords(String[] keyWords) {
        KeyWords = keyWords;
    }

    public String getLocation() {
        return Location;
    }

    public void setLocation(String location) {
        Location = location;
    }

    public String getExternalEventLink() {
        return ExternalEventLink;
    }

    public void setExternalEventLink(String externalEventLink) {
        ExternalEventLink = externalEventLink;
    }
}
