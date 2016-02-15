package com.example.acty.droid3.DBObjects;

public class CampaignMedia
{
    private String userId;
    private byte[] data;
    private String fileName;
    private String contentType;
    private int contentLength;

    public String getUserId()
    {
        return userId;
    }

    public void setUserId(String value)
    {
        userId = value;
    }

    public byte[] getData()
    {
        return data;
    }

    public void setData(byte[] value)
    {
        data = value;
    }

    public String getFileName()
    {
        return fileName;
    }

    public void setFileName(String value)
    {
        fileName = value;
    }

    public String getContentType()
    {
        return contentType;
    }

    public void setContentType(String value)
    {
        contentType = value;
    }

    public int getContentLength()
    {
        return contentLength;
    }

    public void setContentLength(int value)
    {
        contentLength = value;
    }
}
