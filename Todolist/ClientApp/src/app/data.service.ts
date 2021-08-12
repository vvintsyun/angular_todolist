import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Task } from './task';
import { TaskList } from './taskList';
import {UpdateTaskCompletedDto} from './task-list/task-list.component';

@Injectable()
export class TaskDataService {

  private url = "/api/tasks";

  constructor(private http: HttpClient) {
  }

  getTasks(tasklistid: number) {
    return this.http.get(this.url + '/byTasklistid/' + tasklistid);
  }

  getTasksByUrl(url: string) {
    return this.http.get(this.url + '/byTasklistUrl/' + url);
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

  updateCompleted(taskDto: UpdateTaskCompletedDto) {
    return this.http.put(this.url + '/updateCompleted', taskDto);
  }
}

@Injectable()
export class TaskListDataService {

  private url = "/api/tasklists";

  constructor(private http: HttpClient) {
  }

  getTaskListUrl(id: number) {
    return this.http.get(this.url + '/geturl/' + id);
  }

  getAllTaskLists() {
    return this.http.get(this.url);
  }

  getTaskListById(id: number) {
    return this.http.get(this.url + '/' + id);
  }

  getTaskListByUrl(url: string) {
    return this.http.get(this.url + '/TaskListByUrl/' + url);
  }

  createTaskList(tasklist: TaskList) {
    return this.http.post(this.url, tasklist);
  }

  updateTaskList(tasklist: TaskList) {
    return this.http.put(this.url + '/' + tasklist.id, tasklist);
  }

  deleteTaskList(id: number) {
    return this.http.delete(this.url + '/' + id);
  }

  downloadZip() {
    location.href = this.url + '/downloadzip';
  }
}
