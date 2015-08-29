package com.example.acty.droid3;

import android.app.Activity;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AbsListView;
import android.widget.AdapterView;
import android.widget.ListAdapter;
import android.widget.TextView;


import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;

/**
 * A fragment representing a list of Items.
 * <p/>
 * Large screen devices (such as tablets) are supported by replacing the ListView
 * with a GridView.
 * <p/>
 * Activities containing this fragment MUST implement the {@link OnFeedFragmentInteractionListener}
 * interface.
 */
public class FeedFragment extends Fragment implements AbsListView.OnItemClickListener {

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFeedFragmentInteractionListener mListener;

    /**
     * The fragment's ListView/GridView.
     */
    private AbsListView mListView;

    /**
     * The Adapter which will be used to populate the ListView/GridView with
     * Views.
     */
    private CampaignListAdapter mAdapter;

    public static FeedFragment newInstance(String param1, String param2) {
        FeedFragment fragment = new FeedFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    /**
     * Mandatory empty constructor for the fragment manager to instantiate the
     * fragment (e.g. upon screen orientation changes).
     */
    public FeedFragment() {
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }

        // TODO: Change Adapter to display your content
        mAdapter = new CampaignListAdapter(getActivity(), new ArrayList<DBCampaign>());
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_feed, container, false);

        new GetFeedData().execute(mParam1);

        // Set the adapter
        mListView = (AbsListView) view.findViewById(android.R.id.list);
        ((AdapterView<ListAdapter>) mListView).setAdapter(mAdapter);

        // Set OnItemClickListener so we can be notified on item clicks
        mListView.setOnItemClickListener(this);

        return view;
    }

    @Override
    public void onAttach(Activity activity) {
        super.onAttach(activity);
        try {
            mListener = (OnFeedFragmentInteractionListener) activity;
        } catch (ClassCastException e) {
            throw new ClassCastException(activity.toString()
                    + " must implement OnFragmentInteractionListener");
        }
    }

    @Override
    public void onDetach() {
        super.onDetach();
        mListener = null;
    }

    @Override
    public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
        if (null != mListener) {
            // Notify the active callbacks interface (the activity, if the
            // fragment is attached to one) that an item has been selected.
            mListener.onFeedFragmentInteraction(DummyContent.ITEMS.get(position).id);
        }
    }

    /**
     * The default content for this Fragment has a TextView that is shown when
     * the list is empty. If you would like to change the text, call this method
     * to supply the text it should use.
     */
    public void setEmptyText(CharSequence emptyText) {
        View emptyView = mListView.getEmptyView();

        if (emptyView instanceof TextView) {
            ((TextView) emptyView).setText(emptyText);
        }
    }

    /**
     * This interface must be implemented by activities that contain this
     * fragment to allow an interaction in this fragment to be communicated
     * to the activity and potentially other fragments contained in that
     * activity.
     * <p/>
     * See the Android Training lesson <a href=
     * "http://developer.android.com/training/basics/fragments/communicating.html"
     * >Communicating with Other Fragments</a> for more information.
     */
    public interface OnFeedFragmentInteractionListener {
        // TODO: Update argument type and name
        public void onFeedFragmentInteraction(String id);
    }

    public class GetFeedData extends AsyncTask<String, Void, List<DBCampaign>> {

        private final String LOG_TAG = GetFeedData.class.getSimpleName();

        @Override
        protected List<DBCampaign> doInBackground(String... params) {
            String userName = params[0];
            // These two need to be declared outside the try/catch
            // so that they can be closed in the finally block.
            HttpURLConnection urlConnection = null;
            BufferedReader reader = null;

            // Will contain the raw JSON response as a string.
            String campaignsJsonStr = null;
            JSONObject obj;
            List<DBCampaign> returnArray = null;
            try {
                Uri builder = Uri.parse(String.format("http://acty.azurewebsites.net/ActiDataService.svc/GetTopFeedsForUser/userName/%s", userName));

                URL url = new URL(builder.toString());
                Log.v(LOG_TAG, "Build Uri" + builder.toString());
                // Create the request to OpenWeatherMap, and open the connection
                urlConnection = (HttpURLConnection) url.openConnection();
                urlConnection.setRequestMethod("GET");
                urlConnection.connect();

                // Read the input stream into a String
                InputStream inputStream = urlConnection.getInputStream();
                StringBuffer buffer = new StringBuffer();
                if (inputStream == null) {
                    // Nothing to do.
                    return null;
                }
                reader = new BufferedReader(new InputStreamReader(inputStream));

                String line;
                while ((line = reader.readLine()) != null) {
                    // Since it's JSON, adding a newline isn't necessary (it won't affect parsing)
                    // But it does make debugging a *lot* easier if you print out the completed
                    // buffer for debugging.
                    buffer.append(line + "\n");
                }

                if (buffer.length() == 0) {
                    // Stream was empty.  No point in parsing.
                    return null;
                }
                campaignsJsonStr = buffer.toString();
                returnArray = getCampaignsFromJson(campaignsJsonStr);
            } catch (IOException e) {
                Log.e(LOG_TAG, e.getMessage(), e);
                // If the code didn't successfully get the weather data, there's no point in attemping
                // to parse it.
                return null;
            } catch (JSONException e) {
                Log.e(LOG_TAG, e.getMessage(), e);
                // If the code didn't successfully get the weather data, there's no point in attemping
                // to parse it.
                return null;
            }finally {
                if (urlConnection != null) {
                    urlConnection.disconnect();
                }
                if (reader != null) {
                    try {
                        reader.close();
                    } catch (final IOException e) {
                        Log.e(LOG_TAG, "Error closing stream", e);
                    }
                }
            }

            //return campaignsJsonStr;
            return returnArray;
        }

        /**
         * Take the String representing the complete array of campaigns in JSON Format and
         * pull out the data we need to construct the Strings needed for the wireframes.
         *
         * Fortunately parsing is easy:  constructor takes the JSON string and converts it
         * into an Object hierarchy for us.
         */
        private List<DBCampaign> getCampaignsFromJson(String campaignsJsonStr)
                throws JSONException {

            // These are the names of the JSON objects that need to be extracted.
            final String OWM_Category = "Category";
            final String OWM_CampaignMediaResourceBlob = "StoryMediaResourceBlob";
            final String OWM_CreatedDate = "CreatedDate";
            final String OWM_OwnerId = "OwnerId";
            final String OWM_Heading = "Heading";
            final String OWM_Message = "Message";
            final String OWM_IsLocal = "IsLocal";
            final String OWM_ZipCode = "ZipCode";
            final String OWM_Country = "Country";
            final String OWM_KeyWords = "KeyWords";
            final String OWM_LastUpdatedDate = "LastUpdatedDate";
            final String OWM_CommentsCount = "CommentsCount";
            final String OWM_participationCount = "participationCount";
            final String OWM_Events = "Events";
            final String OWM_Status = "Status";
            final String OWM_id = "id";

            JSONArray campaignsJsonArray = new JSONArray(campaignsJsonStr);

            int len = campaignsJsonArray.length();
            List<DBCampaign> resultStrs = new ArrayList<DBCampaign>(len);
            for(int i = 0; i < len; i++) {
                // Get the JSON object representing a campaign
                JSONObject campaignJSON = campaignsJsonArray.getJSONObject(i);
                DBCampaign camp = new DBCampaign();

                // Currently ownerId and OwnerName are pointing to same thing. we have to change this.
                String ownerId = campaignJSON.getString(OWM_OwnerId);
                camp.setOwnerId(ownerId);
                camp.setOwnerName(ownerId);

                camp.setCategory(CampaignCategory.valueOf(campaignJSON.getString(OWM_Category)));
                camp.setStatus(CampaignStatus.valueOf(campaignJSON.getString(OWM_Status)));

                camp.setHeading(campaignJSON.getString(OWM_Heading));
                camp.setCampaignId(campaignJSON.getString(OWM_id));
                camp.setIsLocal(campaignJSON.getBoolean(OWM_IsLocal));
                camp.setMessage(campaignJSON.getString(OWM_Message));
                camp.setZipCode(campaignJSON.getString(OWM_ZipCode));
                camp.setCreatedDate(campaignJSON.getString(OWM_CreatedDate));
                camp.setLastUpdatedDate(campaignJSON.getString(OWM_LastUpdatedDate));
                camp.setCountry(campaignJSON.getString(OWM_Country));
                camp.setCampaignMediaResourceBlob(campaignJSON.getString(OWM_CampaignMediaResourceBlob));
                camp.setCommentsCount(campaignJSON.getInt(OWM_CommentsCount));
                camp.setParticipationCount(campaignJSON.getInt(OWM_participationCount));

                JSONArray keywordJsonArr = campaignJSON.optJSONArray(OWM_KeyWords);
                if (keywordJsonArr != null) {
                    int keyWordsLen = keywordJsonArr.length();
                    String[] keywords = null;
                    if (keyWordsLen > 0) {
                        keywords = new String[keyWordsLen];
                        for (int j = 0; j < keyWordsLen; j++) {
                            keywords[j] = (String) keywordJsonArr.get(j);
                        }
                    }
                    camp.setKeyWords(keywords);
                }


                JSONArray eventsJsonArr = campaignJSON.optJSONArray(OWM_Events);
                if (eventsJsonArr != null) {
                    int eventsLen = eventsJsonArr.length();
                    String[] events = null;
                    if (eventsLen > 0) {
                        events = new String[eventsLen];
                        for (int j = 0; j < eventsLen; j++) {
                            events[j] = (String) eventsJsonArr.get(j);
                        }
                    }
                    camp.setEvents(events);
                }
                resultStrs.add(camp);
            }

            return resultStrs;
        }

        @Override
        protected void onPostExecute(List<DBCampaign> campaigns) {

            if (campaigns == null)
                return;

            mAdapter.clear();
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB) {
                mAdapter.addAll(campaigns);
            }
            else {
                for (DBCampaign c: campaigns)
                    mAdapter.add(c);
            }
            //weekForecastAdapter.clear();
            //weekForecastAdapter.addAll(campaigns);
            /*for (int i = 0; i< strings.length; i++){
                weekForecastAdapter.add(strings[i]);
            }*/

            // weekForecastAdapter.notifyDataSetChanged();
        }
    }
}
