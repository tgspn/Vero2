package com.example.veroapp.adpters

import android.support.v7.widget.RecyclerView
import android.content.Context
import com.example.veroapp.R
import android.os.Parcel
import android.os.Parcelable
import android.support.v7.widget.RecyclerView.Adapter
import android.view.View
import android.view.ViewGroup
import android.view.LayoutInflater
import com.example.veroapp.Database.AppDatabase
import com.example.veroapp.models.KeyModel
import kotlinx.android.synthetic.main.content_keys_item.view.*
import kotlin.concurrent.thread
import kotlin.coroutines.coroutineContext


class KeysAdapter(val context: Context,private var list: MutableList<KeyModel>) : Adapter<KeysAdapter.ViewHolder>() {
    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        val view = LayoutInflater.from(context).inflate(R.layout.content_keys_item, parent, false)
        return ViewHolder(view)
    }

    override fun getItemCount(): Int {
       return list.size
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        thread {
            var item = list[position]
            holder.computerName.text = item.computerName
            holder.date.text = item.date
        }
    }

    class ViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        val computerName = itemView.firstLine
        val date = itemView.secondLine
    }


}