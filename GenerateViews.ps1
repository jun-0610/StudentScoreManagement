$dir = "c:\Users\HP\OneDrive - ptjh.tp.edu.tw\桌面\智慧生產\StudentScoreManagement\Views"
New-Item -ItemType Directory -Force -Path "$dir\Student" | Out-Null
New-Item -ItemType Directory -Force -Path "$dir\Course" | Out-Null
New-Item -ItemType Directory -Force -Path "$dir\Score" | Out-Null

$studentIndex = @"
@model IEnumerable<StudentScoreManagement.Models.Student>
@{ ViewData["Title"] = "學生管理"; }
<h1>學生列表</h1>
<p><a asp-action="Create" class="btn btn-primary">新增學生</a></p>
<table class="table">
    <thead>
        <tr>
            <th>@Html.DisplayNameFor(model => model.StudentName)</th>
            <th>@Html.DisplayNameFor(model => model.Gender)</th>
            <th>@Html.DisplayNameFor(model => model.Department)</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
@foreach (var item in Model) {
        <tr>
            <td>@Html.DisplayFor(modelItem => item.StudentName)</td>
            <td>@Html.DisplayFor(modelItem => item.Gender)</td>
            <td>@Html.DisplayFor(modelItem => item.Department)</td>
            <td>
                <a asp-action="Edit" asp-route-id="@item.StudentID">編輯</a> |
                <a asp-action="Details" asp-route-id="@item.StudentID">詳細</a> |
                <a asp-action="Delete" asp-route-id="@item.StudentID">刪除</a>
            </td>
        </tr>
}
    </tbody>
</table>
"@
Set-Content -Path "$dir\Student\Index.cshtml" -Value $studentIndex -Encoding UTF8

$studentCreate = @"
@model StudentScoreManagement.Models.Student
@{ ViewData["Title"] = "新增學生"; }
<h1>新增學生</h1>
<hr />
<div class="row">
    <div class="col-md-4">
        <form asp-action="Create">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            <div class="form-group mb-3">
                <label asp-for="StudentName" class="control-label"></label>
                <input asp-for="StudentName" class="form-control" />
                <span asp-validation-for="StudentName" class="text-danger"></span>
            </div>
            <div class="form-group mb-3">
                <label asp-for="Gender" class="control-label"></label>
                <input asp-for="Gender" class="form-control" />
                <span asp-validation-for="Gender" class="text-danger"></span>
            </div>
            <div class="form-group mb-3">
                <label asp-for="Department" class="control-label"></label>
                <input asp-for="Department" class="form-control" />
                <span asp-validation-for="Department" class="text-danger"></span>
            </div>
            <div class="form-group">
                <input type="submit" value="儲存" class="btn btn-primary" />
            </div>
        </form>
    </div>
</div>
<div><a asp-action="Index">返回列表</a></div>
"@
Set-Content -Path "$dir\Student\Create.cshtml" -Value $studentCreate -Encoding UTF8

$studentEdit = @"
@model StudentScoreManagement.Models.Student
@{ ViewData["Title"] = "編輯學生"; }
<h1>編輯學生</h1>
<hr />
<div class="row">
    <div class="col-md-4">
        <form asp-action="Edit">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            <input type="hidden" asp-for="StudentID" />
            <div class="form-group mb-3">
                <label asp-for="StudentName" class="control-label"></label>
                <input asp-for="StudentName" class="form-control" />
                <span asp-validation-for="StudentName" class="text-danger"></span>
            </div>
            <div class="form-group mb-3">
                <label asp-for="Gender" class="control-label"></label>
                <input asp-for="Gender" class="form-control" />
                <span asp-validation-for="Gender" class="text-danger"></span>
            </div>
            <div class="form-group mb-3">
                <label asp-for="Department" class="control-label"></label>
                <input asp-for="Department" class="form-control" />
                <span asp-validation-for="Department" class="text-danger"></span>
            </div>
            <div class="form-group">
                <input type="submit" value="儲存" class="btn btn-primary" />
            </div>
        </form>
    </div>
</div>
<div><a asp-action="Index">返回列表</a></div>
"@
Set-Content -Path "$dir\Student\Edit.cshtml" -Value $studentEdit -Encoding UTF8

$studentDetails = @"
@model StudentScoreManagement.Models.Student
@{ ViewData["Title"] = "學生詳細資料"; }
<h1>學生詳細資料</h1>
<div>
    <hr />
    <dl class="row">
        <dt class="col-sm-2">@Html.DisplayNameFor(model => model.StudentName)</dt>
        <dd class="col-sm-10">@Html.DisplayFor(model => model.StudentName)</dd>
        <dt class="col-sm-2">@Html.DisplayNameFor(model => model.Gender)</dt>
        <dd class="col-sm-10">@Html.DisplayFor(model => model.Gender)</dd>
        <dt class="col-sm-2">@Html.DisplayNameFor(model => model.Department)</dt>
        <dd class="col-sm-10">@Html.DisplayFor(model => model.Department)</dd>
    </dl>
</div>
<hr />
<h3>修課紀錄</h3>
<table class="table table-hover">
    <thead>
        <tr>
            <th>課程名稱</th>
            <th>成績</th>
            <th>學期</th>
        </tr>
    </thead>
    <tbody>
        @if (Model.Scores != null && Model.Scores.Any())
        {
            foreach (var score in Model.Scores)
            {
                <tr>
                    <td>@(score.Course != null ? score.Course.CourseName : "無課程資料")</td>
                    <td>@score.ScoreValue</td>
                    <td>@score.Semester</td>
                </tr>
            }
        }
        else
        {
            <tr>
                <td colspan="3" class="text-center text-muted">尚無修課資料</td>
            </tr>
        }
    </tbody>
</table>
<div class="mt-3">
    <a asp-action="Edit" asp-route-id="@Model.StudentID" class="btn btn-primary">編輯</a> |
    <a asp-action="Index" class="btn btn-secondary">返回列表</a>
</div>
"@
Set-Content -Path "$dir\Student\Details.cshtml" -Value $studentDetails -Encoding UTF8

$studentDelete = @"
@model StudentScoreManagement.Models.Student
@{ ViewData["Title"] = "刪除學生"; }
<h1>刪除學生</h1>
<h3>確定要刪除這筆資料嗎？</h3>
<div>
    <hr />
    <dl class="row">
        <dt class="col-sm-2">@Html.DisplayNameFor(model => model.StudentName)</dt>
        <dd class="col-sm-10">@Html.DisplayFor(model => model.StudentName)</dd>
        <dt class="col-sm-2">@Html.DisplayNameFor(model => model.Gender)</dt>
        <dd class="col-sm-10">@Html.DisplayFor(model => model.Gender)</dd>
        <dt class="col-sm-2">@Html.DisplayNameFor(model => model.Department)</dt>
        <dd class="col-sm-10">@Html.DisplayFor(model => model.Department)</dd>
    </dl>
    <form asp-action="Delete">
        <input type="hidden" asp-for="StudentID" />
        <input type="submit" value="刪除" class="btn btn-danger" /> |
        <a asp-action="Index">返回列表</a>
    </form>
</div>
"@
Set-Content -Path "$dir\Student\Delete.cshtml" -Value $studentDelete -Encoding UTF8


# Course Views
$courseIndex = @"
@model IEnumerable<StudentScoreManagement.Models.Course>
@{ ViewData["Title"] = "課程管理"; }
<h1>課程列表</h1>
<p><a asp-action="Create" class="btn btn-primary">新增課程</a></p>
<table class="table">
    <thead>
        <tr>
            <th>@Html.DisplayNameFor(model => model.CourseName)</th>
            <th>@Html.DisplayNameFor(model => model.Credits)</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
@foreach (var item in Model) {
        <tr>
            <td>@Html.DisplayFor(modelItem => item.CourseName)</td>
            <td>@Html.DisplayFor(modelItem => item.Credits)</td>
            <td>
                <a asp-action="Edit" asp-route-id="@item.CourseID">編輯</a> |
                <a asp-action="Details" asp-route-id="@item.CourseID">詳細</a> |
                <a asp-action="Delete" asp-route-id="@item.CourseID">刪除</a>
            </td>
        </tr>
}
    </tbody>
</table>
"@
Set-Content -Path "$dir\Course\Index.cshtml" -Value $courseIndex -Encoding UTF8

$courseCreate = @"
@model StudentScoreManagement.Models.Course
@{ ViewData["Title"] = "新增課程"; }
<h1>新增課程</h1>
<hr />
<div class="row">
    <div class="col-md-4">
        <form asp-action="Create">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            <div class="form-group mb-3">
                <label asp-for="CourseName" class="control-label"></label>
                <input asp-for="CourseName" class="form-control" />
                <span asp-validation-for="CourseName" class="text-danger"></span>
            </div>
            <div class="form-group mb-3">
                <label asp-for="Credits" class="control-label"></label>
                <input asp-for="Credits" class="form-control" />
                <span asp-validation-for="Credits" class="text-danger"></span>
            </div>
            <div class="form-group">
                <input type="submit" value="儲存" class="btn btn-primary" />
            </div>
        </form>
    </div>
</div>
<div><a asp-action="Index">返回列表</a></div>
"@
Set-Content -Path "$dir\Course\Create.cshtml" -Value $courseCreate -Encoding UTF8

$courseEdit = @"
@model StudentScoreManagement.Models.Course
@{ ViewData["Title"] = "編輯課程"; }
<h1>編輯課程</h1>
<hr />
<div class="row">
    <div class="col-md-4">
        <form asp-action="Edit">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            <input type="hidden" asp-for="CourseID" />
            <div class="form-group mb-3">
                <label asp-for="CourseName" class="control-label"></label>
                <input asp-for="CourseName" class="form-control" />
                <span asp-validation-for="CourseName" class="text-danger"></span>
            </div>
            <div class="form-group mb-3">
                <label asp-for="Credits" class="control-label"></label>
                <input asp-for="Credits" class="form-control" />
                <span asp-validation-for="Credits" class="text-danger"></span>
            </div>
            <div class="form-group">
                <input type="submit" value="儲存" class="btn btn-primary" />
            </div>
        </form>
    </div>
</div>
<div><a asp-action="Index">返回列表</a></div>
"@
Set-Content -Path "$dir\Course\Edit.cshtml" -Value $courseEdit -Encoding UTF8

$courseDetails = @"
@model StudentScoreManagement.Models.Course
@{ ViewData["Title"] = "課程詳細資料"; }
<h1>課程詳細資料</h1>
<div>
    <hr />
    <dl class="row">
        <dt class="col-sm-2">@Html.DisplayNameFor(model => model.CourseName)</dt>
        <dd class="col-sm-10">@Html.DisplayFor(model => model.CourseName)</dd>
        <dt class="col-sm-2">@Html.DisplayNameFor(model => model.Credits)</dt>
        <dd class="col-sm-10">@Html.DisplayFor(model => model.Credits)</dd>
    </dl>
</div>
<div>
    <a asp-action="Edit" asp-route-id="@Model.CourseID" class="btn btn-primary">編輯</a> |
    <a asp-action="Index">返回列表</a>
</div>
"@
Set-Content -Path "$dir\Course\Details.cshtml" -Value $courseDetails -Encoding UTF8

$courseDelete = @"
@model StudentScoreManagement.Models.Course
@{ ViewData["Title"] = "刪除課程"; }
<h1>刪除課程</h1>
<h3>確定要刪除這筆資料嗎？</h3>
<div>
    <hr />
    <dl class="row">
        <dt class="col-sm-2">@Html.DisplayNameFor(model => model.CourseName)</dt>
        <dd class="col-sm-10">@Html.DisplayFor(model => model.CourseName)</dd>
        <dt class="col-sm-2">@Html.DisplayNameFor(model => model.Credits)</dt>
        <dd class="col-sm-10">@Html.DisplayFor(model => model.Credits)</dd>
    </dl>
    <form asp-action="Delete">
        <input type="hidden" asp-for="CourseID" />
        <input type="submit" value="刪除" class="btn btn-danger" /> |
        <a asp-action="Index">返回列表</a>
    </form>
</div>
"@
Set-Content -Path "$dir\Course\Delete.cshtml" -Value $courseDelete -Encoding UTF8


# Score Views
$scoreIndex = @"
@model IEnumerable<StudentScoreManagement.Models.Score>
@{ ViewData["Title"] = "成績管理"; }
<h1>成績列表</h1>
<p><a asp-action="Create" class="btn btn-primary">新增成績</a></p>
<table class="table">
    <thead>
        <tr>
            <th>@Html.DisplayNameFor(model => model.Student)</th>
            <th>@Html.DisplayNameFor(model => model.Course)</th>
            <th>@Html.DisplayNameFor(model => model.ScoreValue)</th>
            <th>@Html.DisplayNameFor(model => model.Semester)</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
@foreach (var item in Model) {
        <tr>
            <td>@Html.DisplayFor(modelItem => item.Student.StudentName)</td>
            <td>@Html.DisplayFor(modelItem => item.Course.CourseName)</td>
            <td>@Html.DisplayFor(modelItem => item.ScoreValue)</td>
            <td>@Html.DisplayFor(modelItem => item.Semester)</td>
            <td>
                <a asp-action="Edit" asp-route-id="@item.ScoreID">編輯</a> |
                <a asp-action="Details" asp-route-id="@item.ScoreID">詳細</a> |
                <a asp-action="Delete" asp-route-id="@item.ScoreID">刪除</a>
            </td>
        </tr>
}
    </tbody>
</table>
"@
Set-Content -Path "$dir\Score\Index.cshtml" -Value $scoreIndex -Encoding UTF8

$scoreCreate = @"
@model StudentScoreManagement.Models.Score
@{ ViewData["Title"] = "新增成績"; }
<h1>新增成績</h1>
<hr />
<div class="row">
    <div class="col-md-4">
        <form asp-action="Create">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            <div class="form-group mb-3">
                <label asp-for="StudentID" class="control-label"></label>
                <select asp-for="StudentID" class="form-control" asp-items="ViewBag.StudentID"></select>
            </div>
            <div class="form-group mb-3">
                <label asp-for="CourseID" class="control-label"></label>
                <select asp-for="CourseID" class="form-control" asp-items="ViewBag.CourseID"></select>
            </div>
            <div class="form-group mb-3">
                <label asp-for="ScoreValue" class="control-label"></label>
                <input asp-for="ScoreValue" class="form-control" />
                <span asp-validation-for="ScoreValue" class="text-danger"></span>
            </div>
            <div class="form-group mb-3">
                <label asp-for="Semester" class="control-label"></label>
                <input asp-for="Semester" class="form-control" />
                <span asp-validation-for="Semester" class="text-danger"></span>
            </div>
            <div class="form-group">
                <input type="submit" value="儲存" class="btn btn-primary" />
            </div>
        </form>
    </div>
</div>
<div><a asp-action="Index">返回列表</a></div>
"@
Set-Content -Path "$dir\Score\Create.cshtml" -Value $scoreCreate -Encoding UTF8

$scoreEdit = @"
@model StudentScoreManagement.Models.Score
@{ ViewData["Title"] = "編輯成績"; }
<h1>編輯成績</h1>
<hr />
<div class="row">
    <div class="col-md-4">
        <form asp-action="Edit">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            <input type="hidden" asp-for="ScoreID" />
            <div class="form-group mb-3">
                <label asp-for="StudentID" class="control-label"></label>
                <select asp-for="StudentID" class="form-control" asp-items="ViewBag.StudentID"></select>
                <span asp-validation-for="StudentID" class="text-danger"></span>
            </div>
            <div class="form-group mb-3">
                <label asp-for="CourseID" class="control-label"></label>
                <select asp-for="CourseID" class="form-control" asp-items="ViewBag.CourseID"></select>
                <span asp-validation-for="CourseID" class="text-danger"></span>
            </div>
            <div class="form-group mb-3">
                <label asp-for="ScoreValue" class="control-label"></label>
                <input asp-for="ScoreValue" class="form-control" />
                <span asp-validation-for="ScoreValue" class="text-danger"></span>
            </div>
            <div class="form-group mb-3">
                <label asp-for="Semester" class="control-label"></label>
                <input asp-for="Semester" class="form-control" />
                <span asp-validation-for="Semester" class="text-danger"></span>
            </div>
            <div class="form-group">
                <input type="submit" value="儲存" class="btn btn-primary" />
            </div>
        </form>
    </div>
</div>
<div><a asp-action="Index">返回列表</a></div>
"@
Set-Content -Path "$dir\Score\Edit.cshtml" -Value $scoreEdit -Encoding UTF8

$scoreDetails = @"
@model StudentScoreManagement.Models.Score
@{ ViewData["Title"] = "成績詳細資料"; }
<h1>成績詳細資料</h1>
<div>
    <hr />
    <dl class="row">
        <dt class="col-sm-2">@Html.DisplayNameFor(model => model.Student)</dt>
        <dd class="col-sm-10">@Html.DisplayFor(model => model.Student.StudentName)</dd>
        <dt class="col-sm-2">@Html.DisplayNameFor(model => model.Course)</dt>
        <dd class="col-sm-10">@Html.DisplayFor(model => model.Course.CourseName)</dd>
        <dt class="col-sm-2">@Html.DisplayNameFor(model => model.ScoreValue)</dt>
        <dd class="col-sm-10">@Html.DisplayFor(model => model.ScoreValue)</dd>
        <dt class="col-sm-2">@Html.DisplayNameFor(model => model.Semester)</dt>
        <dd class="col-sm-10">@Html.DisplayFor(model => model.Semester)</dd>
    </dl>
</div>
<div>
    <a asp-action="Edit" asp-route-id="@Model.ScoreID" class="btn btn-primary">編輯</a> |
    <a asp-action="Index">返回列表</a>
</div>
"@
Set-Content -Path "$dir\Score\Details.cshtml" -Value $scoreDetails -Encoding UTF8

$scoreDelete = @"
@model StudentScoreManagement.Models.Score
@{ ViewData["Title"] = "刪除成績"; }
<h1>刪除成績</h1>
<h3>確定要刪除這筆資料嗎？</h3>
<div>
    <hr />
    <dl class="row">
        <dt class="col-sm-2">@Html.DisplayNameFor(model => model.Student)</dt>
        <dd class="col-sm-10">@Html.DisplayFor(model => model.Student.StudentName)</dd>
        <dt class="col-sm-2">@Html.DisplayNameFor(model => model.Course)</dt>
        <dd class="col-sm-10">@Html.DisplayFor(model => model.Course.CourseName)</dd>
        <dt class="col-sm-2">@Html.DisplayNameFor(model => model.ScoreValue)</dt>
        <dd class="col-sm-10">@Html.DisplayFor(model => model.ScoreValue)</dd>
        <dt class="col-sm-2">@Html.DisplayNameFor(model => model.Semester)</dt>
        <dd class="col-sm-10">@Html.DisplayFor(model => model.Semester)</dd>
    </dl>
    <form asp-action="Delete">
        <input type="hidden" asp-for="ScoreID" />
        <input type="submit" value="刪除" class="btn btn-danger" /> |
        <a asp-action="Index">返回列表</a>
    </form>
</div>
"@
Set-Content -Path "$dir\Score\Delete.cshtml" -Value $scoreDelete -Encoding UTF8
