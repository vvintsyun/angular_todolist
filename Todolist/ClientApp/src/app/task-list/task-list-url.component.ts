import { Component, OnInit } from '@angular/core';
import {Task} from '../task';
import {TaskDataService, TasklistDataService} from '../data.service';
import {ActivatedRoute} from '@angular/router';
import { Tasklist } from '../tasklist';

@Component({
  selector: 'app-task-list-url',
  templateUrl: './task-list-url.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListUrlComponent implements OnInit {

  tasks: Task[];
  tasklist: Tasklist;
  tasklisturl: string;
  isEmpty: boolean = true;

  constructor(private taskDataService: TaskDataService, private taskListDataService: TasklistDataService, private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const url = params.get('url');
      if (url) {
        this.tasklisturl = url;
        this.loadTasklist(url);
        this.loadTasks(url);
      }
    });
  }

  loadTasklist(tasklistUrl: string) {
    this.taskListDataService.getTasklistByUrl(tasklistUrl)
      .subscribe((data: Tasklist) => this.tasklist = data );
  }

  loadTasks(tasklistUrl: string) {
    this.taskDataService.getTasksByUrl(tasklistUrl)
      .subscribe((data: Task[]) => {
        this.tasks = data;
        this.isEmpty = this.tasks.length === 0;
      });
  }

  changeCompleted(task: Task) {
    task.iscompleted = !task.iscompleted;
    this.taskDataService.updateTask(task)
      .subscribe(data => this.loadTasks(this.tasklisturl));
  }
}
