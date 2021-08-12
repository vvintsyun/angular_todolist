import { Component, OnInit, OnDestroy } from '@angular/core';
import {Task} from '../task';
import {TaskDataService, TaskListDataService} from '../data.service';
import {ActivatedRoute} from '@angular/router';
import { SecurityService } from '../login.service';
import { TaskList } from '../taskList';
import { PlatformLocation } from '@angular/common';
import {Subscription} from 'rxjs';
import {switchMap} from 'rxjs/operators';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit, OnDestroy {

  subscription: Subscription;
  editableTask: Task = new Task();
  tasks: Task[];
  tableMode: boolean = true;
  taskList: TaskList;
  taskListId: number;
  isEmpty: boolean = true;
  generatedTaskListUrl: string;
  readonly urlPath: string;
  taskListUrl: string;

  constructor(private taskDataService: TaskDataService,
              private taskListDataService: TaskListDataService,
              private route: ActivatedRoute,
              private accountService: SecurityService,
              private location: PlatformLocation) {
    this.urlPath = (location as any).location.origin;
  }

  ngOnInit() {
    //fix incorrect (not existed/not permitted) id behaviour
    this.subscription = this.route.paramMap.subscribe((params) => {
      const id = params.get('id');
      if (id) {
        this.taskListId = +id;
        this.loadTaskList(this.taskListId)
          .subscribe((data: TaskList) => this.loadTaskListSubscribtion(data));
        this.loadTasks(this.taskListId)
          .subscribe((data: Task[]) => this.loadTasksSubscribtion(data));
        this.generatedTaskListUrl = undefined;
      }
      const url = params.get('url');
      if (url) {
        this.taskListUrl = url;
        this.loadTaskListByUrl(this.taskListUrl)
          .subscribe((data: TaskList) => this.loadTaskListSubscribtion(data));
        this.loadTasksByUrl(this.taskListUrl)
          .subscribe((data: Task[]) => this.loadTasksSubscribtion(data));
      }
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  getUrl() {
    this.taskListDataService.getTaskListUrl(this.taskListId)
      .subscribe(x => {
        this.generatedTaskListUrl = this.urlPath + '/tasklistbyurl/' + x['url'];
      });
  }

  loadTaskList(taskListId: number) {
    return this.taskListDataService.getTaskListById(taskListId);
  }

  loadTasks(taskListId: number) {
    return this.taskDataService.getTasks(taskListId);
  }

  loadTaskListByUrl(taskListUrl: string) {
    return this.taskListDataService.getTaskListByUrl(taskListUrl);
  }

  loadTasksByUrl(taskListUrl: string) {
    return this.taskDataService.getTasksByUrl(taskListUrl);
  }

  loadTasksSubscribtion(data: Task[]) {
    this.tasks = data;
    this.isEmpty = this.tasks.length === 0;
  }

  loadTaskListSubscribtion(data: TaskList) {
    this.taskList = data;
  }

  changeCompleted(task: Task) {
    task.isCompleted = !task.isCompleted;
    const updateDto = new UpdateTaskCompletedDto(task.id, task.isCompleted);
    this.taskDataService.updateCompleted(updateDto)
      .pipe(switchMap(_ => {
        return this.taskListId
          ? this.loadTasks(this.taskListId)
          : this.loadTasksByUrl(this.taskListUrl);
      }))
      .subscribe((data: Task[]) => this.loadTasksSubscribtion(data));
  }

  save() {
    if (this.editableTask.id == null) {
      this.editableTask.taskListId = this.taskList.id;
      this.taskDataService.createTask(this.editableTask)
        .pipe(switchMap(_ => this.loadTasks(this.taskListId)))
        .subscribe((data: Task[]) => this.loadTasksSubscribtion(data));
    } else {
      this.taskDataService.updateTask(this.editableTask)
        .pipe(switchMap(_ => this.loadTasks(this.taskListId)))
        .subscribe((data: Task[]) => this.loadTasksSubscribtion(data));
    }
    this.cancel();
  }

  editTask(t: Task) {
    this.editableTask = Object.assign({}, t);
  }

  cancel() {
    this.editableTask = new Task();
    this.tableMode = true;
  }

  delete(t: Task) {
    this.taskDataService.deleteTask(t.id)
      .pipe(switchMap(_ => this.loadTasks(this.taskListId)))
      .subscribe((data: Task[]) => this.loadTasksSubscribtion(data));
  }

  add() {
    this.cancel();
    this.tableMode = false;
  }
}

export class UpdateTaskCompletedDto {
  constructor(
    public id: number,
    public isCompleted: boolean) { }
}
