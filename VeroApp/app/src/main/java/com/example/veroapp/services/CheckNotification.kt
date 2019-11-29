package com.example.veroapp.services

import android.app.Activity
import android.app.AlertDialog
import android.app.job.JobParameters
import android.app.job.JobService
import android.arch.persistence.room.Room
import android.content.Context
import android.content.DialogInterface
import android.content.Intent
import android.widget.Toast
import com.example.veroapp.*
import com.example.veroapp.Database.AppDatabase
import com.example.veroapp.models.RequestUserModel
import com.fasterxml.jackson.module.kotlin.jacksonObjectMapper
import com.fasterxml.jackson.module.kotlin.readValue
import kotlinx.android.synthetic.main.content_key_manager.*
import java.util.*
import java.util.logging.Handler
import kotlin.collections.HashMap
import kotlin.concurrent.thread
import kotlin.concurrent.timerTask



class CheckNotification : JobService() {
    override fun onStopJob(params: JobParameters?): Boolean {
        //TODO("not implemented") //To change body of created functions use File | Settings | File Templates.
        return true
    }
    var activity: Activity? =null
    fun register(activity: Activity) {
        this.activity=activity
    }
    override fun onStartJob(parameters: JobParameters?): Boolean {
        // runs on the main thread, so this Toast will appear
        Toast.makeText(this, "test", Toast.LENGTH_SHORT).show()
        // perform work here, i.e. network calls asynchronously
        //var activity = this.applicationContext
        var database = AppDatabase.getInstance(this)
        val mapper = jacksonObjectMapper()


//        while (true) {
        Timer().schedule(timerTask {
            thread {
                database.keyDAO().all().forEach {
                    val url = getString(R.string.server_endpoint) + "api/info/" + it.id
//                    var header=HashMap<String,String>()
//                    header.put("Origin","https://54.159.123.153:4433")
                    val response = khttp.get(url)
                    if (response.statusCode == 200 && response.text != null) {
                        try {
//                            val model = mapper.readValue<RequestUserModel>(response.text)
                            activity.run{
                                ShowAlert(response.text, activity)
                            }
                        } catch (e: Exception) {
                            e.printStackTrace()
                        }

                    }
                }
            }
        }, 1000, 1000)
//            val url = getString(R.string.server_endpoint) + "api/wallet"
//
//            val response = khttp.post(url)
//        }
        // returning false means the work has been done, return true if the job is being run asynchronously
        return true
    }

    private fun ShowAlert(requestUserModel: String, activity: Activity?) {


        val intent = Intent(this, UserNotification::class.java);
        intent.putExtra("modelText",requestUserModel)
        startActivity(intent);

    }


}