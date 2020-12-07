import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FetcherService } from '../fetcher.service';
import { Project } from '../models/project';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent implements OnInit {
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
}
