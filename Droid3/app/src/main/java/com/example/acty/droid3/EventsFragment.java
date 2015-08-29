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
 * Activities containing this fragment MUST implement the {@link OnEventsFragmentInteractionListener}
 * interface.
 */
public class EventsFragment extends Fragment implements AbsListView.OnItemClickListener {

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnEventsFragmentInteractionListener mListener;

    /**
     * The fragment's ListView/GridView.
     */
    private AbsListView mListView;

    /**
     * The Adapter which will be used to populate the ListView/GridView with
     * Views.
     */
    private EventListAdapter mAdapter;

    // TODO: Rename and change types of parameters
    public static EventsFragment newInstance(String param1, String param2) {
        EventsFragment fragment = new EventsFragment();
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
    public EventsFragment() {
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }

        mAdapter = new EventListAdapter(getActivity(), new ArrayList<DBEvent>());
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_events, container, false);

        new GetEventsData().execute(mParam1);
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
            mListener = (OnEventsFragmentInteractionListener) activity;
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
            mListener.onEventsFragmentInteraction(DummyContent.ITEMS.get(position).id);
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
    public interface OnEventsFragmentInteractionListener {
        // TODO: Update argument type and name
        public void onEventsFragmentInteraction(String id);
    }

    public class GetEventsData extends AsyncTask<String, Void, List<DBEvent>> {

        private final String LOG_TAG = GetEventsData.class.getSimpleName();

        @Override
        protected List<DBEvent> doInBackground(String... params) {
            String userName = params[0];
            // These two need to be declared outside the try/catch
            // so that they can be closed in the finally block.
            HttpURLConnection urlConnection = null;
            BufferedReader reader = null;

            // Will contain the raw JSON response as a string.
            String eventsJsonStr = null;
            JSONObject obj;
            List<DBEvent> returnArray = null;
            try {
                Uri builder = Uri.parse(String.format("http://acty.azurewebsites.net/ActiDataService.svc/GetEventsForUser/userName/%s", userName));

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
                eventsJsonStr = buffer.toString();
                returnArray = getEventsFromJson(eventsJsonStr);
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
        private List<DBEvent> getEventsFromJson(String campaignsJsonStr)
                throws JSONException {

            // These are the names of the JSON objects that need to be extracted.
            final String OWM_CampaignId = "CampaignId";
            final String OWM_CreatedDate = "CreatedDate";
            final String OWM_Followers = "Followers";
            final String OWM_Comments = "Comments";
            final String OWM_OwnerId = "OwnerId";
            final String OWM_Description = "Description";
            final String OWM_Time = "Time";
            final String OWM_Location = "Location";
            final String OWM_ZipCode = "ZipCode";
            final String OWM_Country = "Country";
            final String OWM_KeyWords = "KeyWords";
            final String OWM_LastUpdatedDate = "LastUpdatedDate";
            final String OWM_ExternalEventLink = "ExternalEventLink";
            final String OWM_id = "id";

            JSONArray campaignsJsonArray = new JSONArray(campaignsJsonStr);

            int len = campaignsJsonArray.length();
            List<DBEvent> resultStrs = new ArrayList<DBEvent>(len);
            for(int i = 0; i < len; i++) {
                // Get the JSON object representing a campaign
                JSONObject campaignJSON = campaignsJsonArray.getJSONObject(i);
                DBEvent evt = new DBEvent();

                // Currently ownerId and OwnerName are pointing to same thing. we have to change this.
                String ownerId = campaignJSON.getString(OWM_OwnerId);
                evt.setOwnerId(ownerId);

                evt.setDescription(campaignJSON.getString(OWM_Description));
                evt.setTime(campaignJSON.getString(OWM_Time));

                evt.setLocation(campaignJSON.getString(OWM_Location));
                evt.setCampaignId(campaignJSON.getString(OWM_CampaignId));
                evt.setZipCode(campaignJSON.getString(OWM_ZipCode));
                evt.setCreatedDate(campaignJSON.getString(OWM_CreatedDate));
                evt.setLastUpdatedDate(campaignJSON.getString(OWM_LastUpdatedDate));
                evt.setCountry(campaignJSON.getString(OWM_Country));
                evt.setExternalEventLink(campaignJSON.getString(OWM_ExternalEventLink));
                evt.setId(campaignJSON.getString(OWM_id));

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
                    evt.setKeyWords(keywords);
                }


                JSONArray followersJsonArr = campaignJSON.optJSONArray(OWM_Followers);
                if (followersJsonArr != null) {
                    int followersLen = followersJsonArr.length();
                    List<String> followers = new ArrayList<String>(followersLen);
                    if (followersLen > 0) {
                        for (int j = 0; j < followersLen; j++) {
                            followers.add((String) followersJsonArr.get(j));
                        }
                    }
                    evt.setFollowers(followers);
                }

                JSONArray commentsJsonArr = campaignJSON.optJSONArray(OWM_Comments);
                if (commentsJsonArr != null) {
                    int commentsLen = commentsJsonArr.length();
                    List<String> comments = new ArrayList<String>(commentsLen);
                    if (commentsLen > 0) {
                        for (int j = 0; j < commentsLen; j++) {
                            comments.add((String) commentsJsonArr.get(j));
                        }
                    }
                    evt.setFollowers(comments);
                }

                resultStrs.add(evt);
            }

            return resultStrs;
        }

        @Override
        protected void onPostExecute(List<DBEvent> events) {

            if (events == null)
                return;

            mAdapter.clear();
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB) {
                mAdapter.addAll(events);
            }
            else {
                for (DBEvent e: events)
                    mAdapter.add(e);
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
