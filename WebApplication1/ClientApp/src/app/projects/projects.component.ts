import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FetcherService } from '../fetcher.service';
import { Project } from '../models/project';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css']
})
export class ProjectsComponent implements OnInit {
  public Projects: Array<Project>

  constructor(private fetcher: FetcherService, private authorizeService: AuthorizeService, private router: Router) { 
  }

  ngOnInit() {
    this.authorizeService.isAuthenticated().subscribe(auth => {
      if (!auth) {
        this.router.navigate(["/authentication/login"]);
      } else {
        this.getProjects();
      }
    });
  }

  getProjects() {
    this.fetcher.getProjectsFromApi(sessionStorage.getItem('username')).subscribe(resp => this.Projects=resp, error => console.log(error));
  }

  getProject(idx: number) {
    this.router.navigate(["project", this.Projects[idx].id ]);
  }

}
