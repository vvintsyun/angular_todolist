import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Task } from './task';
import { Tasklist } from './tasklist';
 
@Injectable()
export class TaskDataService {
 
    private url = "/api/tasks";
 
    constructor(private http: HttpClient) {
    }
 
    getTasks() {
        return this.http.get(this.url);
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

  getTasklists() {
    return this.http.get(this.url);
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
}
