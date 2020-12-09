import { SubTask } from "./sub-task";

export class Task {
    public id: string;
    public name: string;
    public project: string;
    public priority: string;
    public type: string;
    public status: string;
    public handledBy: string[];
    public users: string;
    public subTasks: string;
    public description: string;
    public serverSideTaskList: SubTask[];
    public dateOfCreation: Date;
    public endDate: Date;
    public numberOfSubTasks: number;
}
