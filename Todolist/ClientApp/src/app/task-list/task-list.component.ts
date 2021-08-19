import { Component, OnInit, OnDestroy } from '@angular/core';
import {Task} from '../task';
import {TaskDataService, TaskListDataService} from '../services/data.service';
import {ActivatedRoute} from '@angular/router';
import { SecurityService } from '../services/login.service';
import { TaskList } from '../taskList';
import { PlatformLocation } from '@angular/common';
import {of, Subscription, throwError, zip} from 'rxjs';
import {catchError, switchMap} from 'rxjs/operators';
import {NotificationService} from '../services/error-notification.service';

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
  id: number;

  constructor(private taskDataService: TaskDataService,
              private taskListDataService: TaskListDataService,
              private notification: NotificationService,
              private route: ActivatedRoute,
              private accountService: SecurityService,
              private location: PlatformLocation) {
    this.urlPath = (location as any).location.origin;
  }

  ngOnInit() {
    this.subscription = this.route.paramMap.pipe(
      switchMap(params => {
        this.taskListId = +params.get('id');
        if (this.taskListId) {
          this.generatedTaskListUrl = undefined;
          return zip(this.loadTaskList(this.taskListId), this.loadTasks(this.taskListId))
            .pipe(catchError(() => {
              this.notification.showError('Error on getting data was occured.');
              return of();
            }));
        }
        this.taskListUrl = params.get('url');
        if (this.taskListUrl) {
          return zip(this.loadTaskListByUrl(this.taskListUrl), this.loadTasksByUrl(this.taskListUrl))
            .pipe(catchError(() => {
              this.notification.showError('Error on getting data was occured.');
              return of();
            }));
        }
      }))
      .subscribe(data => {
        this.loadTaskListSubscription(data[0]);
        this.loadTasksSubscription(data[1]);
      });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  getUrl() {
    this.taskListDataService.getTaskListUrl(this.taskListId)
      .subscribe(x => {
        this.generatedTaskListUrl = this.urlPath + '/tasklistbyurl/' + x['url'];
      },
      (_ => {
        this.notification.showError('Error on getting task list was occured.');
      }));
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

  loadTasksSubscription(data) {
    this.tasks = data;
    this.isEmpty = this.tasks.length === 0;
  }

  loadTaskListSubscription(data: TaskList) {
    this.taskList = data;
  }

  changeCompleted(task: Task) {
    task.isCompleted = !task.isCompleted;
    const updateDto = new UpdateTaskCompletedDto(task.id, task.isCompleted);
    this.taskDataService.updateCompleted(updateDto)
      .pipe(catchError(err => {
          this.notification.showError('Error on updating task was occured.');
          return of();
        }),
        switchMap(_ => {
          return this.taskListId
            ? this.loadTasks(this.taskListId)
            : this.loadTasksByUrl(this.taskListUrl);
        }))
      .subscribe((data: Task[]) => this.loadTasksSubscription(data),
        (_ => {
          this.notification.showError('Error on getting tasks was occured.');
        }));
  }

  save() {
    if (this.editableTask.id == null) {
      this.editableTask.taskListId = this.taskList.id;
      this.taskDataService.createTask(this.editableTask)
        .pipe(catchError(err => {
            this.notification.showError('Error on creating task was occured.');
            return of();
          }),
          switchMap(_ => this.loadTasks(this.taskListId)))
        .subscribe((data: Task[]) => this.loadTasksSubscription(data),
          (_ => {
            this.notification.showError('Error on getting tasks was occured.');
          }));
    } else {
      this.taskDataService.updateTask(this.editableTask)
        .pipe(catchError(err => {
            this.notification.showError('Error on updating task was occured.');
            return of();
          }),
          switchMap(_ => this.loadTasks(this.taskListId)))
        .subscribe((data: Task[]) => this.loadTasksSubscription(data),
          (_ => {
            this.notification.showError('Error on getting tasks was occured.');
          }));
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
      .pipe(catchError(err => {
          this.notification.showError('Error on deleting task was occured.');
          return of();
        }),
        switchMap(_ => this.loadTasks(this.taskListId)))
      .subscribe((data: Task[]) => this.loadTasksSubscription(data),
        (_ => {
          this.notification.showError('Error on getting tasks was occured.');
        }));
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
