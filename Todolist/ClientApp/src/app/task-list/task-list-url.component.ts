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
  tasklistid: number;
  showHeader: boolean;

  constructor(private taskDataService: TaskDataService, private taskListDataService: TasklistDataService, private route: ActivatedRoute) {
  }

  ngOnInit() {    
    this.route.paramMap.subscribe((params) => {
      this.tasklisturl = params.get('url');
      this.taskListDataService.getTasklistIdbyUrl(this.tasklisturl).subscribe((id: number) => {
        this.tasklistid = id;
        this.loadTasklist(id);
        this.loadTasks(id);
      });      
    });
  } 
  
  loadTasklist(tasklistid: number) {
    this.taskListDataService.getTasklist(tasklistid)
      .subscribe((data: Tasklist) => this.tasklist = data );
  }

  loadTasks(tasklistid: number) {
    this.taskDataService.getTasks(tasklistid)
      .subscribe((data: Task[]) => { this.tasks = data; this.showHeader = this.tasks.length > 0; });
  }

  changeCompleted(task: Task) {
    task.iscompleted = !task.iscompleted;
    this.taskDataService.updateTask(task)
      .subscribe(data => this.loadTasks(this.tasklistid));
  }
}
