package com.example.acty.droid3;

import android.app.Activity;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.AsyncTask;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.webkit.WebView;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import com.example.acty.droid3.DBObjects.DBCampaign;

import java.io.BufferedInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.URL;
import java.net.URLConnection;
import java.util.List;

import javax.net.ssl.HttpsURLConnection;

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
        ImageView imageView = (ImageView) rowView.findViewById(R.id.icon);
        TextView extratxt = (TextView) rowView.findViewById(R.id.textView1);

        DBCampaign camp = getItem(position);

        if (camp != null) {
            String heading = camp.getHeading();
            if (heading != null)
                    txtTitle.setText(heading);

            // Task XXXXX : We have to detect if the url is an Image or a video and extract the
            //              bitmap for the thumbnail.
            String contentType = camp.getContentType();
            String blobUrl = camp.getCampaignMediaResourceBlob();
            if (contentType != null && contentType.contains("image") && blobUrl != null) {

                GetImageBitmap asyncImageGetter = new GetImageBitmap();
                asyncImageGetter.SetImageView(imageView);
                asyncImageGetter.execute(blobUrl);
            }
            else
                imageView.setVisibility(View.GONE);


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

    public class GetImageBitmap extends AsyncTask<String, Void, Bitmap> {

        private final String LOG_TAG = GetImageBitmap.class.getSimpleName();
        private ImageView iv;

        @Override
        protected Bitmap doInBackground(String... params) {
            String url = params[0];
            // These two need to be declared outside the try/catch
            // so that they can be closed in the finally block.
            Bitmap bm = null;
            try {
                URL aURL = new URL(url);
                URLConnection conn = aURL.openConnection();
                conn.connect();
                InputStream is = conn.getInputStream();
                BufferedInputStream bis = new BufferedInputStream(is);
                bm = BitmapFactory.decodeStream(bis);
                bis.close();
                is.close();
            } catch (IOException e) {
                Log.e(LOG_TAG, "Error getting bitmap", e);
            }
            return bm;
        }

        protected void SetImageView(ImageView iv) {
            this.iv = iv;
        }

        @Override
        protected void onPostExecute(Bitmap bm) {

            if (bm == null)
                return;

            iv.setVisibility(View.VISIBLE);
            iv.setImageBitmap(bm);
        }
    }
}
