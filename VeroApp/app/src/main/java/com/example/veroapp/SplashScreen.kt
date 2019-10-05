package com.example.veroapp

import android.content.Intent
import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.os.Environment
import java.io.File

class SplashScreen : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_splash_screen)
        val fileName = applicationInfo.dataDir+"/hfc-key-store"
        var fileObject = File(fileName)

        if(!fileObject.exists()|| fileObject.list().isEmpty())
        {
           intent = Intent(this, StartActivity::class.java);
        }else{
           intent = Intent(this, MainActivity::class.java);
        }


        startActivity(intent);
    }
}
