package com.example.acty.droid3;

import android.app.Activity;
import android.net.Uri;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.webkit.WebView;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.List;

/**
 * Created by Hemanth on 8/1/2015.
 */
public class CampaignListAdapter extends ArrayAdapter<DBCampaign> {

    private final Activity context;

    public CampaignListAdapter(Activity context, List<DBCampaign> campaigns) {
        super(context, R.layout.fragment_campaign_list, campaigns);

        this.context=context;
    }

    public View getView(int position,View view,ViewGroup parent) {
        LayoutInflater inflater=context.getLayoutInflater();
        View rowView=inflater.inflate(R.layout.fragment_campaign_list, null, true);

        TextView txtTitle = (TextView) rowView.findViewById(R.id.item);
        WebView webView = (WebView) rowView.findViewById(R.id.icon);
        TextView extratxt = (TextView) rowView.findViewById(R.id.textView1);

        DBCampaign camp = getItem(position);

        if (camp != null) {
            String heading = camp.getHeading();
            if (heading != null)
                    txtTitle.setText(heading);

            // Task XXXXX : We have to detect if the url is an Image or a video and extract the
            //              bitmap for the thumbnail.
            String blobUrl = camp.getCampaignMediaResourceBlob();
            if (blobUrl != null)
                webView.loadUrl(blobUrl);

            String msg = camp.getMessage();
            if (msg != null)
                extratxt.setText(msg);
        }
        return rowView;

    }

    /*
    @Override
    public void clear()
    {
        this.campaigns = null;
    }

    public void addAll(DBCampaign[] campaigns) {
        this.campaigns = campaigns;
        this.notifyDataSetChanged();
    }
    */
}
