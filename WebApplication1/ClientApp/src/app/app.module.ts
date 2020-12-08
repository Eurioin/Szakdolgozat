import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { ProjectComponent } from './project/project.component';
import { ProjectsComponent } from './projects/projects.component';
import { HomeComponent } from './home/home.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { AccountComponent } from './account/account.component';
import { AccountsComponent } from './accounts/accounts.component';
import { TaskComponent } from './task/task.component';
import { UpdateProjectComponent } from './update-project/update-project.component';
import { UpdateTaskComponent } from './update-task/update-task.component';
import { CreateProjectComponent } from './create-project/create-project.component';
import { CreateTaskComponent } from './create-task/create-task.component';

@NgModule({
  declarations: [
    AppComponent,
    AccountComponent,
    AccountsComponent,
    CreateProjectComponent,
    CreateTaskComponent,
    HomeComponent,
    NavMenuComponent,
    ProjectComponent,
    ProjectsComponent,
    TaskComponent,
    UpdateProjectComponent,
    UpdateTaskComponent
  ],
  imports: [
    ApiAuthorizationModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'account', component: AccountComponent },
      { path: 'accounts', component: AccountsComponent },
      { path: 'projects', component: ProjectsComponent },
      { path: 'project/:id', component: ProjectComponent },
      { path: 'project/update/:id', component: UpdateProjectComponent },
      { path: 'task/:id', component: TaskComponent },
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
