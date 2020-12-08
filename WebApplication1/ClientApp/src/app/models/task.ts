export class Task {
    public id: string;
    public name: string;
    public project: string;
    public priority: string;
    public type: string;
    public status: string;
    public handledBy: string[];
    public description: string;
    public subTasks: string[];
    public dateOfCreation: Date;
    public endDate: Date;
    public numberOfSubTasks: number;
}