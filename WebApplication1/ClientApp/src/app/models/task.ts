import { SubTask } from "./sub-task";
import { Megjegyzes } from "./comment";

export class Task {
    public id: string;
    public name: string;
    public project: string;
    public priority: string;
    public type: string;
    public status: string;
    public handledBy: string[];
    public users: string;
    public subTasks: SubTask[];
    public description: string;
    public dateOfCreation: Date;
    public endDate: Date;
    public numberOfSubTasks: number;
    public comments: Megjegyzes;
}
