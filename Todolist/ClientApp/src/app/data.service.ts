import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Task } from './task';
import { Tasklist } from './tasklist';
 
@Injectable()
export class TaskDataService {
 
    private url = "/api/tasks";
 
    constructor(private http: HttpClient) {
    }
 
    getTasks(tasklistid: number) {
      return this.http.get(this.url + '/byTasklistid', {params: { "tasklistid": tasklistid.toString()} });
    }
 
    createTask(task: Task) {
        return this.http.post(this.url, task);
    }
    updateTask(task: Task) {  
      return this.http.put(this.url + '/' + task.id, task);
    }
    deleteTask(id: number) {
        return this.http.delete(this.url + '/' + id);
    }
}

@Injectable()
export class TasklistDataService {

  private url = "/api/tasklists";

  constructor(private http: HttpClient) {
  }

  getTasklistUrl(id: number) {
    return this.http.get<string>(this.url + '/geturl', { params: { "tasklistid": id.toString() } });
  }

  getTasklists(userid: string) {
    return this.http.get(this.url + '/byUserid', { params: { "userid": userid} });
  }

  getTasklist(id: number) {
    return this.http.get(this.url + '/' + id);
  }

  createTasklist(tasklist: Tasklist) {
    return this.http.post(this.url, tasklist);
  }
  updateTasklist(tasklist: Tasklist) {
    return this.http.put(this.url + '/' + tasklist.id, tasklist);
  }
  deleteTasklist(id: number) {
    return this.http.delete(this.url + '/' + id);
  }
  downloadlists(userid: string) {
    //return this.http.post(this.url + '/downloadzip', userid);
    location.href = this.url + '/downloadzip/?userid=' + userid;
  }
}
