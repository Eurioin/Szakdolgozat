import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FetcherService } from '../fetcher.service';
import { SubTask } from '../models/sub-task';
import { Task } from '../models/task';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css']
})
export class TaskComponent implements OnInit {
  public Task: Task = new Task();
  public SubTasks: Array<SubTask> = [];
  public Handlers: Array<string> = [];
  constructor(private fetcher: FetcherService, private authorizeService: AuthorizeService, private router: Router, private route: ActivatedRoute) { 
  }

  ngOnInit() {
    this.authorizeService.isAuthenticated().subscribe(auth => {
      if (!auth) {
        this.router.navigate(["/authentication/login"]);
      } else {
        this.fetcher.getTaskFromApi(this.route.snapshot.paramMap.get('id')).subscribe(resp => {
          this.Task = resp;
          this.Task.serverSideTaskList.forEach( sb => {
            var f = this.SubTasks.filter(s =>s.id === sb.id);
            if (f.length === 0) {
              var sub = new SubTask();
              sub.id = sb.id;
              sub.description = sb.description;
              this.SubTasks.push(sub);
            }
          });
          this.Task.handledBy.forEach(h => {
            this.fetcher.getAccountFromApiById(h).subscribe(r => {
              var customstr = r.name + " - " + r.email;
              if (customstr.length > 3 && !this.Handlers.includes(customstr)) {
                this.Handlers.push(r.name + " - " + r.email)
              }
            });
          })
        });
      }
    });
  }

  edit() {
    this.router.navigate(["task/update", this.Task.id]);
  }

  back() {
    this.router.navigate(["project", this.Task.project]);
  }

  delete() {
    var proj = this.Task.project;
    this.fetcher.deleteTaskUsingApi(this.Task).subscribe(resp => this.router.navigate(["project", proj]), err => console.log(err));
  }
}
