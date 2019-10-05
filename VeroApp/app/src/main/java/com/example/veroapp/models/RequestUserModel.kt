package com.example.veroapp.models

import android.os.Parcelable
import java.io.Serializable

class RequestUserModel(
    var id:String,
    var storeName:String,
    var fields:List<String>,
    var value:Double,
    var response: HashMap<String, String>?

) {
}