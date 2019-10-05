package com.example.veroapp

import android.support.multidex.MultiDexApplication
import com.journeyapps.barcodescanner.SourceData

class KtApplication: MultiDexApplication(){
    override fun onCreate() {
        super.onCreate()
//        SourceData.rotateCameraPreview(90,null,100,100)
    }
}