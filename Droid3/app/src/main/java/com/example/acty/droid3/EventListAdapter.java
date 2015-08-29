package com.example.acty.droid3;

import android.app.Activity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.webkit.WebView;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import java.util.List;

/**
 * Created by Hemanth on 8/23/2015.
 */
public class EventListAdapter extends ArrayAdapter<DBEvent> {
    private final Activity context;

    public EventListAdapter(Activity context, List<DBEvent> events) {
        super(context, R.layout.fragment_campaign_list, events);

        this.context = context;
    }

    public View getView(int position, View view, ViewGroup parent) {
        LayoutInflater inflater = context.getLayoutInflater();
        View rowView = inflater.inflate(R.layout.fragment_campaign_list, null, true);

        TextView txtTitle = (TextView) rowView.findViewById(R.id.item);
        WebView webView = (WebView) rowView.findViewById(R.id.icon);
        TextView extratxt = (TextView) rowView.findViewById(R.id.textView1);

        DBEvent evt = getItem(position);

        if (evt != null) {
            String heading = evt.getTime();
            if (heading != null)
                txtTitle.setText(heading);

            // Task XXXXX : We have to detect if the url is an Image or a video and extract the
            //              bitmap for the thumbnail.
            // String blobUrl = evt.getCampaignMediaResourceBlob();
            // if (blobUrl != null)
            //    webView.loadUrl(blobUrl);

            String msg = evt.getDescription();
            if (msg != null)
                extratxt.setText(msg);
        }
        return rowView;
    }
}
