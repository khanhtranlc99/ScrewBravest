package sg.bigo.ads;

import android.app.Activity;
import android.content.Context;
import android.os.Handler;
import android.os.Looper;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.FrameLayout;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import sg.bigo.ads.api.AdOptionsView;
import sg.bigo.ads.api.AdTag;
import sg.bigo.ads.api.MediaView;
import sg.bigo.ads.api.NativeAd;

public class AdHelper {

    public static void postToAndroidMainThread(Runnable runnable) {
        new Handler(Looper.getMainLooper()).post(runnable);
    }

    public static void addAdView(Activity activity, View adView, int position) {
        if (adView == null) return;
        ViewGroup contentView = activity.findViewById(android.R.id.content);
        String tag = "ad_container";
        ViewGroup adContainer = contentView.findViewWithTag(tag);
        if (adContainer == null) {
            adContainer = new FrameLayout(activity);
            adContainer.setTag(tag);
        }
        contentView.removeView(adContainer);
        FrameLayout.LayoutParams layoutParams = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT, position);
        contentView.addView(adContainer, layoutParams);
        adContainer.removeAllViews();
        adContainer.addView(adView);
    }

    public static void removeAdView(Activity activity)
    {
        ViewGroup contentView = activity.findViewById(android.R.id.content);
        String tag = "ad_container";
        ViewGroup adContainer = contentView.findViewWithTag(tag);
        if (adContainer == null) return;
        adContainer.removeAllViews();
    }

    public static int getLayoutIdByResName(Activity activity, String resName) {
        return activity.getResources().getIdentifier(resName, "layout", activity.getPackageName());
    }

    public static int getDrawableIdByResName(Activity activity, String resName) {
        return activity.getResources().getIdentifier(resName, "drawable", activity.getPackageName());
    }

    public static View renderNativeAdView(Activity activity, NativeAd nativeAd, String layoutResName) {
        int layoutId = getLayoutIdByResName(activity, layoutResName);
        if (layoutId <= 0) {
            Log.w("BigoAds-Unity", "Invalid res name: " + layoutResName);
            return null;
        }
        View view = LayoutInflater.from(activity).inflate(layoutId, null, false);
        if (!(view instanceof ViewGroup)) {
            return view;
        }
        ViewGroup nativeView = (ViewGroup) view;
        TextView titleView = findViewByIdName(nativeView, "native_title");
        TextView descriptionView = findViewByIdName(nativeView, "native_description");
        TextView warningView = findViewByIdName(nativeView, "native_warning");
        Button ctaButton = findViewByIdName(nativeView, "native_cta");
        MediaView mediaView = findViewByIdName(nativeView, "native_media_view");
        ImageView iconView = findViewByIdName(nativeView, "native_icon_view");
        AdOptionsView optionsView = findViewByIdName(nativeView, "native_option_view");

        titleView.setTag(AdTag.TITLE);
        descriptionView.setTag(AdTag.DESCRIPTION);
        warningView.setTag(AdTag.WARNING);
        ctaButton.setTag(AdTag.CALL_TO_ACTION);

        titleView.setText(nativeAd.getTitle());
        descriptionView.setText(nativeAd.getDescription());
        warningView.setText(nativeAd.getWarning());
        ctaButton.setText(nativeAd.getCallToAction());

        List<View> clickableViews = new ArrayList<>();
        clickableViews.add(titleView);
        clickableViews.add(descriptionView);
        clickableViews.add(ctaButton);
        nativeAd.registerViewForInteraction(nativeView, mediaView, iconView, optionsView, clickableViews);
        return nativeView;
    }

    private static <T extends View> T findViewByIdName(ViewGroup parent, String name) {
        Context context = parent.getContext();
        int id = context.getResources().getIdentifier(name, "id", context.getPackageName());
        return parent.findViewById(id);
    }
}
