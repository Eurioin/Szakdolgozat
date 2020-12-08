import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FetcherService } from '../fetcher.service';
import { Project } from '../models/project';
import { Task } from '../models/task';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent implements OnInit {
  public Project: Project;
  public WaitingTasks: Array<Task> = [];
  public InWorkTasks: Array<Task> = [];
  public StuckTasks: Array<Task> = [];
  public CompletedTasks: Array<Task> = [];
  public Columns = ["Waiting for begin","In work","Stuck","Completed"];

  constructor(private fetcher: FetcherService, private authorizeService: AuthorizeService, private router: Router, private route: ActivatedRoute) { 
  }

  ngOnInit() {
    this.authorizeService.isAuthenticated().subscribe(auth => {
      if (!auth) {
        this.router.navigate(["/authentication/login"]);
      } else {
        this.fetcher.getProjectFromApi(this.route.snapshot.paramMap.get('id')).subscribe(resp => {
          this.Project = resp;
          this.getTasksFromApi();
        });
      }
    });
  }

  getTasksFromApi() {
    this.Project.tasks.forEach(t => {
      this.fetcher.getTaskFromApi(t).subscribe(task => {
        var filtered = this.WaitingTasks.filter(ta => ta.id === task.id).concat(this.InWorkTasks.filter(ta => ta.id === task.id).concat(this.StuckTasks.filter(ta => ta.id === task.id).concat(this.CompletedTasks.filter(ta => ta.id === task.id))));
        if (filtered.length == 0) {
          switch (task.status) {
            default:
            case 'Waiting for begin':
              this.WaitingTasks.push(task);
              break;
            case 'In work':
              this.InWorkTasks.push(task);
              break;
            case 'Stuck':
              this.StuckTasks.push(task);
              break;
            case 'Completed':
              this.CompletedTasks.push(task);
              break;
          }
        }
      });
    });
  }

  getWaiting(idx: number) {
    this.router.navigate(["task", this.WaitingTasks[idx].id ]);
  }

  getInWork(idx: number) {
    this.router.navigate(["task", this.InWorkTasks[idx].id ]);
  }

  getStuck(idx: number) {
    this.router.navigate(["task", this.StuckTasks[idx].id ]);
  }

  getCompleted(idx: number) {
    this.router.navigate(["task", this.CompletedTasks[idx].id ]);
  }
}
