Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Linq
Imports Laern_gemini.examples_of_arrays__lists__and_dictionaries
Imports System.Diagnostics.Eventing
Imports System.IO
Imports System.Net.WebRequestMethods
'مـــلاحظات 
'كما ذكرت في استفسارك الأول، في النموذج المتشابك، يتم حفظ oldStudentCopy قبل التعديل [i.31]، ويتم دفعها إلى مكدس التراجع (undoStack) [i.34]. هذا الكائن oldStudentCopy هو نسخة من كائن student2 القديم.
Public Class Design_a_complete_program_for_students_DataGridView3

    ' تعريف المجموعات
    Private studentsData As New BindingList(Of student2)
    Private studentsDictionary As New Dictionary(Of Integer, student2)
    Private uniqueAddresses As New HashSet(Of String)
    Private pendingNewStudentsQueue As New Queue(Of student2) ' قائمة انتظار الطلاب الجدد
    Private undoStack As New Stack(Of student2) ' مكدس التراجع
    Private Const CSV_FILE_PATH As String = "students.csv" ' سيتم حفظه في نفس مجلد ملف EXE للبرنامج,تعريف ثابت لمسار ملف CSV ليكون سهل التعديل
    Private nextStudentID As Integer = 1 ' عداد تلقائي لـ ID الطلاب

    ' دالة مساعدة لعرض رسائل الخطأ وتبسيط التحقق من المدخلات
    Private Function IsInputInvalid(condition As Boolean, errorMessage As String) As Boolean
        If condition Then
            MessageBox.Show(errorMessage, "خطأ في الإدخال", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return True
        Else
            Return False
        End If
    End Function

    ' دالة لمسح حقول الإدخال
    Private Sub ClearInputFields()
        TextBox1.Clear() ' الاسم
        TextBox2.Clear() ' العمر
        TextBox3.Clear() ' العنوان
        TextBox4.Clear() ' سنة الالتحاق
        TextBox5.Clear() ' الصف الدراسي
        TextBox6.Clear() ' المعدل
        TextBox1.Focus() ' التركيز على حقل الاسم
    End Sub

    ' دالة للتحقق من صحة مدخلات الطالب وتحويلها
    Private Function ValidateStudentInput(ByVal name As String, ByVal ageText As String, ByVal address As String, ByVal enrollmentYearText As String, ByVal studentClassText As String, ByVal gradeText As String, ByRef parsedAge As Integer, ByRef parsedEnrollmentYear As Integer, ByRef parsedStudentClass As Integer, ByRef parsedGrade As Double) As Boolean
        Dim isAgeValid As Boolean = Integer.TryParse(ageText.Trim(), parsedAge)
        Dim isEnrollmentYearValid As Boolean = Integer.TryParse(enrollmentYearText.Trim(), parsedEnrollmentYear)
        Dim isStudentClassValid As Boolean = Integer.TryParse(studentClassText.Trim(), parsedStudentClass)
        Dim isGradeValid As Boolean = Double.TryParse(gradeText.Trim(), parsedGrade)

        If IsInputInvalid(String.IsNullOrWhiteSpace(name), "الرجاء إدخال اسم الطالب.") Then Return False
        If IsInputInvalid(Not isAgeValid OrElse parsedAge < 5 OrElse parsedAge > 25, "الرجاء إدخال عمر صحيح (بين 5 و 25).") Then Return False
        If IsInputInvalid(String.IsNullOrWhiteSpace(address), "الرجاء إدخال عنوان الطالب.") Then Return False
        If IsInputInvalid(Not isEnrollmentYearValid OrElse parsedEnrollmentYear < 2000 OrElse parsedEnrollmentYear > Date.Now.Year, $"الرجاء إدخال سنة التحاق صحيحة (بين 2000 و {Date.Now.Year}).") Then Return False
        If IsInputInvalid(Not isStudentClassValid OrElse parsedStudentClass < 1 OrElse parsedStudentClass > 12, "الرجاء إدخال صف دراسي صحيح (بين 1 و 12).") Then Return False
        If IsInputInvalid(Not isGradeValid OrElse parsedGrade < 0 OrElse parsedGrade > 100, "الرجاء إدخال معدل صحيح (بين 0 و 100).") Then Return False

        Return True
    End Function

    ' حدث تحميل الفورم: لربط BindingList بـ DataGridView
    Private Sub Design_a_complete_program_for_students_DataGridView3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StudentsDataGridView.DataSource = studentsData
        ' استدعاء دالة التحميل هنا
        LoadStudentsFromCsv()
        UpdateAddressComboBox() ' <---     ' إضافة العناوين من مجموعة uniqueAddresses إلى ComboBox
        ' يمكنك إخفاء أعمدة معينة إذا لزم الأمر
        ' If studentsDataGridView.Columns.Contains("StudentID") Then
        '     studentsDataGridView.Columns("StudentID").Visible = False
        ' End If
    End Sub

    'دالة لحفظ البيانات إلى ملف CSV
    ' دالة لحفظ بيانات الطلاب إلى ملف CSV
    Private Sub SaveStudentsToCsv()
        Try
            Using writer As New System.IO.StreamWriter(CSV_FILE_PATH)
                ' كتابة رأس الأعمدة (اختياري، لكنه مفيد لفهم الملف)
                writer.WriteLine("StudentID,Name,Age,Address,EnrollmentYear,StudentClass,Grade")

                ' كتابة بيانات كل طالب
                For Each student As student2 In studentsData
                    writer.WriteLine($"{student.StudentID},{EscapeCsvField(student.name)},{student.age}," &
                                 $"{EscapeCsvField(student.address)},{student.enrollmentYear},{student.studentClass},{student.grade}")
                Next
            End Using
            MessageBox.Show("تم حفظ بيانات الطلاب بنجاح في ملف CSV.", "حفظ", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء حفظ البيانات: {ex.Message}", "خطأ في الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' دالة لقراءة بيانات الطلاب من ملف CSV
    Private Sub LoadStudentsFromCsv()
        ' مسح البيانات الحالية قبل التحميل
        studentsData.Clear()
        studentsDictionary.Clear()
        uniqueAddresses.Clear()
        undoStack.Clear() ' مسح سجل التراجع عند تحميل بيانات جديدة
        nextStudentID = 1 ' إعادة تعيين ID

        If Not System.IO.File.Exists(CSV_FILE_PATH) Then
            MessageBox.Show("ملف بيانات الطلاب (CSV) غير موجود. سيتم بدء تشغيل جديد.", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            Using reader As New System.IO.StreamReader(CSV_FILE_PATH)
                ' تخطي سطر رأس الأعمدة إذا كان موجوداً
                If Not reader.EndOfStream Then
                    reader.ReadLine() ' اقرأ وتجاهل السطر الأول (رؤوس الأعمدة)
                End If

                While Not reader.EndOfStream
                    Dim line As String = reader.ReadLine()
                    Dim fields As String() = SplitCsvLine(line) ' استخدام دالة مساعدة لتقسيم سطر CSV

                    If fields.Length = 7 Then ' تأكد من أن هناك 7 حقول متوقعة
                        Try
                            Dim studentID As Integer = Integer.Parse(fields(0).Trim())
                            Dim name As String = UnescapeCsvField(fields(1).Trim())
                            Dim age As Integer = Integer.Parse(fields(2).Trim())
                            Dim address As String = UnescapeCsvField(fields(3).Trim())
                            Dim enrollmentYear As Integer = Integer.Parse(fields(4).Trim())
                            Dim studentClass As Integer = Integer.Parse(fields(5).Trim())
                            Dim grade As Double = Double.Parse(fields(6).Trim())

                            ' إنشاء كائن طالب جديد
                            Dim loadedStudent As New student2(studentID, name, age, address, enrollmentYear, studentClass, grade)

                            ' إضافة الطالب إلى المجموعات
                            studentsData.Add(loadedStudent)
                            studentsDictionary.Add(studentID, loadedStudent)
                            uniqueAddresses.Add(address)

                            ' تحديث nextStudentID لضمان استمرارية الـ IDs
                            If studentID >= nextStudentID Then
                                nextStudentID = studentID + 1
                            End If

                        Catch ex As FormatException
                            MessageBox.Show($"تنسيق بيانات غير صالح في ملف CSV: {line}. سيتم تخطي هذا السطر.", "خطأ في التحميل", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Catch ex As Exception
                            MessageBox.Show($"خطأ غير متوقع أثناء تحليل السطر: {line} - {ex.Message}. سيتم تخطي هذا السطر.", "خطأ في التحميل", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End Try
                    End If
                End While
            End Using
            MessageBox.Show("تم تحميل بيانات الطلاب بنجاح من ملف CSV.", "تحميل", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء تحميل البيانات: {ex.Message}", "خطأ في التحميل", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' دالة مساعدة للتعامل مع الفواصل داخل حقول CSV (مهم جداً)
    Private Function EscapeCsvField(field As String) As String
        If field.Contains(",") OrElse field.Contains(ControlChars.CrLf) OrElse field.Contains("""") Then
            ' إذا كان الحقل يحتوي على فاصلة أو سطر جديد أو علامات اقتباس مزدوجة، يجب إحاطته بعلامات اقتباس مزدوجة
            ' ويجب مضاعفة أي علامات اقتباس مزدوجة داخل الحقل
            Return """" & field.Replace("""", """""") & """"
        Else
            Return field
        End If
    End Function

    ' دالة مساعدة لتحليل سطر CSV مع دعم علامات الاقتباس المزدوجة
    ' دالة مساعدة لتحليل سطر CSV مع دعم علامات الاقتباس المزدوجة
    Private Function SplitCsvLine(line As String) As String()
        Dim fields As New List(Of String)
        Dim inQuotes As Boolean = False
        Dim currentField As New System.Text.StringBuilder()

        For i As Integer = 0 To line.Length - 1
            Dim ch As Char = line(i)

            If ch = """" Then
                If inQuotes AndAlso i + 1 < line.Length AndAlso line(i + 1) = """" Then
                    ' Double quote inside quoted field
                    currentField.Append(ch)
                    i += 1 ' Skip next quote
                Else
                    inQuotes = Not inQuotes
                End If
            ElseIf ch = "," AndAlso Not inQuotes Then
                ' هنا، نضيف الحقل الحالي إلى القائمة، دون استدعاء CleanField الآن
                fields.Add(currentField.ToString())
                currentField.Clear()
            Else
                currentField.Append(ch)
            End If
        Next
        fields.Add(currentField.ToString()) ' Add the last field (بدون CleanField هنا أيضاً)

        ' هنا في النهاية، نقوم بعملية "فك الهروب" وإزالة المسافات البيضاء للحقول
        For i As Integer = 0 To fields.Count - 1
            fields(i) = UnescapeCsvField(fields(i)) ' استدعاء UnescapeCsvField المعدلة
        Next

        Return fields.ToArray()
    End Function
    ' دالة مساعدة لإزالة علامات الاقتباس ومضاعفة علامات الاقتباس الداخلية عند القراءة
    ' دالة مساعدة لإزالة علامات الاقتباس ومضاعفة علامات الاقتباس الداخلية عند القراءة، مع إزالة المسافات البيضاء
    Private Function UnescapeCsvField(field As String) As String
        ' أولاً، قم بإزالة المسافات البيضاء من البداية والنهاية
        field = field.Trim()

        If field.StartsWith("""") AndAlso field.EndsWith("""") Then
            ' Remove surrounding quotes and unescape inner double quotes
            Return field.Substring(1, field.Length - 2).Replace("""""", """")
        Else
            Return field
        End If
    End Function
    ' دالة مساعدة لملء حقول الإدخال من الصف المحدد في DataGridView
    Private Sub UpdateInputFieldsFromGrid()
        If StudentsDataGridView.CurrentRow IsNot Nothing Then
            Dim selectedStudent As student2 = CType(StudentsDataGridView.CurrentRow.DataBoundItem, student2)
            TextBox1.Text = selectedStudent.name
            TextBox2.Text = selectedStudent.age.ToString()
            TextBox3.Text = selectedStudent.address
            TextBox4.Text = selectedStudent.enrollmentYear.ToString()
            TextBox5.Text = selectedStudent.studentClass.ToString()
            TextBox6.Text = selectedStudent.grade.ToString()
            ' (يمكن إضافة حقل لـ StudentID إذا كان مرئياً)
        End If
    End Sub

    ' لمعالجت الطلاب حسب الاول اجى هو الاول بيخرجQueueاجراء خاص ب 
    Private Sub ProcessPendingStudents()
        While pendingNewStudentsQueue.Count > 0
            Dim studentToProcess As student2 = pendingNewStudentsQueue.Dequeue
            ' هنا يمكنك تنفيذ أي منطق لمعالجة الطالب المعلق
            ' مثلاً: حفظه في قاعدة بيانات، إرسال إشعار، إلخ.
            MessageBox.Show($"تمت معالجة الطالب المعلق: {studentToProcess.name} (ID: {studentToProcess.StudentID})", "معالجة طالب معلق", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End While
    End Sub

    ' اجراء لتحديث قائمة العناوين الفريدة في ComboBox
    Private Sub UpdateAddressComboBox()
        ' مسح العناصر الموجودة حالياً في ComboBox
        cmbAddress.Items.Clear()
        ' إضافة العناوين من مجموعة uniqueAddresses إلى ComboBox
        For Each address As String In uniqueAddresses
            cmbAddress.Items.Add(address)
        Next
        ' (اختياري) إضافة خيار فارغ أو توجيهي للمستخدم
        If cmbAddress.Items.Count > 0 Then
            cmbAddress.SelectedIndex = -1 ' لا يتم تحديد أي عنصر بشكل افتراضي
            cmbAddress.Text = "" ' مسح النص المحدد ليبدأ المستخدم من جديد
        End If
    End Sub

    ' حدث الضغط على زر "إضافة طالب"
    Private Sub Button_Add_Click(sender As Object, e As EventArgs) Handles Button_Add.Click
        ' جلب البيانات من حقول الإدخال
        Dim newName As String = TextBox1.Text.Trim()
        Dim newAddress As String = TextBox3.Text.Trim()
        ' متغيرات لاستقبال القيم المحولة بواسطة ByRef
        Dim newAge As Integer
        Dim newEnrollmentYear As Integer
        Dim newStudentClass As Integer
        Dim newGrade As Double

        ' التحقق من صحة المدخلات
        If Not ValidateStudentInput(newName, TextBox2.Text, newAddress, TextBox4.Text, TextBox5.Text, TextBox6.Text, newAge, newEnrollmentYear, newStudentClass, newGrade) Then
            Return ' إذا كانت المدخلات غير صالحة، توقف عن تنفيذ الدالة
        End If

        ' إنشاء كائن student2 جديد
        Dim newStudent As New student2(nextStudentID, newName, newAge, newAddress, newEnrollmentYear, newStudentClass, newGrade)

        ' إضافة الطالب إلى BindingList و Dictionary
        studentsData.Add(newStudent)
        studentsDictionary.Add(nextStudentID, newStudent)
        uniqueAddresses.Add(newAddress) 'لتسجيل هذه العملية للتراجع، يتم إنشاء كائن آخر من student2 عن طريق الانشاء 
        UpdateAddressComboBox() ' <---     ' إضافة العناوين من مجموعة uniqueAddresses إلى ComboBox


        ' *** هنا التعديل لإضافة عملية التراجع ***
        ' نجهز عملية التراجع لتكون "حذف الطالب الذي تم إضافته"
        '''هذا يشير إلى أن فئة student2 في النموذج المتشابك تم تصميمها لتتلقى ActionType.AddStudent
        Dim undoAction As New student2(student2.ActionType.AddStudent, newStudent) 'هذا يعني أن الكائن student2 الذي يُفترض أن يمثل طالبًا، أصبح يحمل أيضًا بيانات تتعلق بعملية التراجع.
        undoStack.Push(undoAction) 'يتم تجهيز عملية التراجع (التي تكون "حذف الطالب الذي تم إضافته") ويتم تمثيلها بكائن student2 جديد، ثم يتم دفع هذا الكائن مباشرة إلى undoStack الخاص بالفورم
        ' ************************************

        nextStudentID += 1 ' زيادة ID للطالب التالي
        ClearInputFields() ' مسح حقول الإدخال
        MessageBox.Show("تمت إضافة الطالب بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' حدث الضغط على زر "تعديل طالب"
    Private Sub Button_Update_Click(sender As Object, e As EventArgs) Handles Button_Update.Click
        If StudentsDataGridView.CurrentRow Is Nothing Then
            MessageBox.Show("الرجاء تحديد طالب لتعديله.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim selectedStudentID As Integer = CInt(StudentsDataGridView.CurrentRow.Cells("StudentID").Value)

        If Not studentsDictionary.ContainsKey(selectedStudentID) Then
            MessageBox.Show("الطالب المحدد غير موجود.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim studentToUpdate As student2 = studentsDictionary(selectedStudentID)
        'بااختصار عملة نفس عمل الدالة Clone
        ' *** حفظ نسخة من بيانات الطالب القديمة قبل التعديل ***
        ' هام جداً: يجب إنشاء نسخة جديدة لتجنب المراجع
        Dim oldStudentCopy As New student2(studentToUpdate.StudentID, studentToUpdate.name, studentToUpdate.age,
                                          studentToUpdate.address, studentToUpdate.enrollmentYear,
                                          studentToUpdate.studentClass, studentToUpdate.grade)
        ' ******************************************************

        ' جلب البيانات الجديدة من حقول الإدخال
        Dim updatedName As String = TextBox1.Text.Trim()
        Dim updatedAddress As String = TextBox3.Text.Trim()
        ' متغيرات لاستقبال القيم المحولة بواسطة ByRef
        Dim updatedAge As Integer
        Dim updatedEnrollmentYear As Integer
        Dim updatedStudentClass As Integer
        Dim updatedGrade As Double

        ' التحقق من صحة المدخلات الجديدة
        If Not ValidateStudentInput(updatedName, TextBox2.Text, updatedAddress, TextBox4.Text, TextBox5.Text, TextBox6.Text, updatedAge, updatedEnrollmentYear, updatedStudentClass, updatedGrade) Then
            Return ' إذا كانت المدخلات غير صالحة، توقف عن تنفيذ الدالة
        End If

        ' تحديث uniqueAddresses إذا تغير العنوان
        If studentToUpdate.address <> updatedAddress Then
            uniqueAddresses.Remove(studentToUpdate.address) ' إزالة العنوان القديم
            uniqueAddresses.Add(updatedAddress) ' إضافة العنوان الجديد
            UpdateAddressComboBox() ' <---     ' إضافة العناوين من مجموعة uniqueAddresses إلى ComboBox

        End If

        ' تحديث خصائص كائن الطالب
        studentToUpdate.name = updatedName
        studentToUpdate.age = updatedAge
        studentToUpdate.address = updatedAddress
        studentToUpdate.enrollmentYear = updatedEnrollmentYear
        studentToUpdate.studentClass = updatedStudentClass
        studentToUpdate.grade = updatedGrade

        ' *** هنا التعديل لإضافة عملية التراجع ***
        ' نجهز عملية التراجع لتكون "استعادة الطالب إلى حالته القديمة"
        Dim undoAction As New student2(student2.ActionType.EditStudent, studentToUpdate, oldStudentCopy)
        undoStack.Push(undoAction)
        UpdateAddressComboBox() ' <---     ' إضافة العناوين من مجموعة uniqueAddresses إلى ComboBox

        ' ************************************

        MessageBox.Show("تم تعديل بيانات الطالب بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' حدث الضغط على زر "حذف طالب"
    Private Sub Button_Delete_Click(sender As Object, e As EventArgs) Handles Button_Delete.Click
        If StudentsDataGridView.CurrentRow Is Nothing Then
            MessageBox.Show("الرجاء تحديد طالب لحذفه.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim result As DialogResult = MessageBox.Show("هل أنت متأكد من حذف هذا الطالب؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Dim selectedStudentID As Integer = CInt(StudentsDataGridView.CurrentRow.Cells("StudentID").Value)

            If studentsDictionary.ContainsKey(selectedStudentID) Then
                Dim studentToDelete As student2 = studentsDictionary(selectedStudentID) '  اجعل المتغير يحمل القيمة الحذف الي تمت من  الكلاس الفرعي بنائن على رقم المعرف بحيث يصبح  المتغير=رقم المتغير
                ' احفظ رقم الفهرس للطالب من القائمة الرئيسية بنائن على رقم المعرف 
                Dim originalIndex As Integer = studentsData.IndexOf(studentToDelete) ' studentToDeleteاجعل المتغير يحمل قيمة الفهرس الاصلي للطالب الي تم حذفة ينائن على المتغير

                ' *** هنا الإضافة عملية التراجع ***
                ' نجهز عملية التراجع لتكون "إعادة إضافة الطالب الذي تم حذفه"
                Dim undoAction As New student2(student2.ActionType.DeleteStudent, studentToDelete, originalIndex) 'originalIndex ,studentToDelete,اجعل الكائن يحمل بيانات العملية الحذف للطالب ثم اسنادها الى الى المتغير  
                undoStack.Push(undoAction)
                ' ************************************

                ' إزالة الطالب من جميع المجموعات
                studentsData.Remove(studentToDelete)
                studentsDictionary.Remove(selectedStudentID)
                uniqueAddresses.Remove(studentToDelete.address) ' إزالة العنوان من المجموعة الفريدة
                UpdateAddressComboBox() ' <---     ' إضافة العناوين من مجموعة uniqueAddresses إلى ComboBox
                ClearInputFields() ' مسح حقول الإدخال بعد الحذف
                MessageBox.Show("تم حذف الطالب بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("الطالب المحدد غير موجود.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    ' حدث الضغط على زر "تراجع" (Undo)
    Private Sub Button_Undo_Click(sender As Object, e As EventArgs) Handles Button_Undo.Click
        If undoStack.Count = 0 Then
            MessageBox.Show("لا توجد عمليات للتراجع عنها.", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim lastAction As student2 = undoStack.Pop() ' اسحب آخر عملية للتراجع عنها

        Select Case lastAction.type
            Case student2.ActionType.AddStudent
                ' إذا كانت العملية الأصلية هي إضافة طالب، فالتراجع يعني حذفه
                If studentsDictionary.ContainsKey(lastAction.StudentData.StudentID) Then
                    Dim studentToRemove As student2 = studentsDictionary(lastAction.StudentData.StudentID)
                    studentsData.Remove(studentToRemove)
                    studentsDictionary.Remove(studentToRemove.StudentID)
                    uniqueAddresses.Remove(studentToRemove.address)
                    MessageBox.Show($"تم التراجع عن إضافة الطالب ID: {studentToRemove.StudentID}.", "تراجع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            Case student2.ActionType.EditStudent
                ' إذا كانت العملية الأصلية هي تعديل طالب، فالتراجع يعني استعادة الحالة القديمة
                If studentsDictionary.ContainsKey(lastAction.StudentData.StudentID) Then
                    Dim studentToRestore As student2 = studentsDictionary(lastAction.StudentData.StudentID)

                    ' تحديث العنوان في uniqueAddresses إذا تغير
                    If studentToRestore.address <> lastAction.OldStudentData.address Then
                        uniqueAddresses.Remove(studentToRestore.address) ' إزالة الجديد
                        uniqueAddresses.Add(lastAction.OldStudentData.address) ' إضافة القديم
                    End If

                    ' تحديث خصائص الطالب بالكائن القديم المحفوظ
                    studentToRestore.name = lastAction.OldStudentData.name
                    studentToRestore.age = lastAction.OldStudentData.age
                    studentToRestore.address = lastAction.OldStudentData.address
                    studentToRestore.enrollmentYear = lastAction.OldStudentData.enrollmentYear
                    studentToRestore.studentClass = lastAction.OldStudentData.studentClass
                    studentToRestore.grade = lastAction.OldStudentData.grade

                    ' تحديث عرض البيانات (BindingList ستقوم بالتحديث التلقائي)
                    MessageBox.Show($"تم التراجع عن تعديل الطالب ID: {studentToRestore.StudentID}.", "تراجع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            Case student2.ActionType.DeleteStudent
                ' إذا كانت العملية الأصلية هي حذف طالب، فالتراجع يعني إعادته
                If Not studentsDictionary.ContainsKey(lastAction.StudentData.StudentID) Then
                    ' إضافة الطالب إلى Dictionary و HashSet أولاً
                    studentsDictionary.Add(lastAction.StudentData.StudentID, lastAction.StudentData)
                    uniqueAddresses.Add(lastAction.StudentData.address)

                    ' إضافة الطالب إلى BindingList في مكانه الأصلي إذا كان الفهرس صالحاً
                    If lastAction.OriginalIndex >= 0 AndAlso lastAction.OriginalIndex <= studentsData.Count Then
                        studentsData.Insert(lastAction.OriginalIndex, lastAction.StudentData)
                    Else
                        studentsData.Add(lastAction.StudentData) ' إذا لم يكن الفهرس صالحاً، أضفه في النهاية
                    End If
                    MessageBox.Show($"تم التراجع عن حذف الطالب ID: {lastAction.StudentData.StudentID}.", "تراجع", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
        End Select
        ' بعد التراجع، قم بتحديث حقول الإدخال لعرض بيانات الطالب الحالي إذا كان هناك صف محدد
        If StudentsDataGridView.CurrentRow IsNot Nothing Then
            UpdateInputFieldsFromGrid()
        Else
            ClearInputFields()
        End If
    End Sub

    ' حدث النقر على صف في DataGridView لملء حقول الإدخال
    Private Sub studentsDataGridView_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles StudentsDataGridView.CellClick
        If e.RowIndex >= 0 Then ' التأكد من أن النقر ليس على رأس العمود
            UpdateInputFieldsFromGrid()
        End If
    End Sub



    ' زر البحث (المبحث الحالي)
    Private Sub Button_Search_Click(sender As Object, e As EventArgs) Handles Button_Search.Click
        Dim searchTerm As String = TextBox_Search.Text.Trim()

        If String.IsNullOrWhiteSpace(searchTerm) Then
            StudentsDataGridView.DataSource = studentsData ' عرض جميع الطلاب إذا كان البحث فارغاً
            Return
        End If

        ' البحث بناءً على الاسم أو العنوان أو ID
        Dim searchResults As New BindingList(Of student2)
        For Each student As student2 In studentsData
            If student.name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 OrElse
               student.address.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 OrElse
               student.StudentID.ToString() = searchTerm Then
                searchResults.Add(student)
            End If
        Next
        StudentsDataGridView.DataSource = searchResults
    End Sub

    ' زر عرض الكل
    Private Sub btnShowAll_Click(sender As Object, e As EventArgs) Handles btnShowAll.Click
        StudentsDataGridView.DataSource = studentsData
        TextBox_Search.Clear()
    End Sub


    ' *** الكود المعدل لـ btnEnqueueStudent_Click ***
    ' زر لإضافة طالب إلى قائمة الانتظار (دون تسجيله رسمياً في النظام بعد)
    Private Sub btnEnqueueStudent_Click(sender As Object, e As EventArgs) Handles btnEnqueueStudent.Click
        ' يجب أن تكون هذه الدالة مرتبطة بزر في واجهة المستخدم، وليست مجرد مثال.

        ' جلب البيانات من حقول الإدخال
        Dim newName As String = TextBox1.Text.Trim()
        Dim newAddress As String = TextBox3.Text.Trim()
        ' متغيرات لاستقبال القيم المحولة بواسطة ByRef
        Dim tempAge As Integer ' نستخدم tempAge لتمييزه عن newAge في btnAddStudent_Click
        Dim tempEnrollmentYear As Integer
        Dim tempStudentClass As Integer
        Dim tempGrade As Double

        ' التحقق من صحة المدخلات
        If Not ValidateStudentInput(newName, TextBox2.Text, newAddress, TextBox4.Text, TextBox5.Text, TextBox6.Text, tempAge, tempEnrollmentYear, tempStudentClass, tempGrade) Then
            Return ' إذا كانت المدخلات غير صالحة، توقف
        End If

        ' إنشاء كائن طالب مؤقت (لا يتم تعيين nextStudentID هنا بعد)
        ' يمكننا تعيين ID مؤقت أو تركه 0 إذا كنا لا نهتم بـ ID في هذه المرحلة
        Dim studentForQueue As New student2(0, newName, tempAge, newAddress, tempEnrollmentYear, tempStudentClass, tempGrade)

        pendingNewStudentsQueue.Enqueue(studentForQueue)

        ' لا تزيد nextStudentID هنا لأن الطالب لم يتم إضافته إلى studentsData/Dictionary بعد
        ' هذا مجرد مثال على وضعهم في طابور انتظار للمعالجة لاحقاً
        ClearInputFields()
        MessageBox.Show($"تمت إضافة الطالب {studentForQueue.name} إلى قائمة المعالجة المعلقة. عدد الطلاب المعلقين: {pendingNewStudentsQueue.Count}", "قائمة الانتظار", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub


    ' *** الكود المعدل لـ btnProcessPendingStudents_Click ***
    ' زر لمعالجة الطلاب من قائمة الانتظار (وتسجيلهم رسمياً)
    Private Sub btnProcessPendingStudents_Click(sender As Object, e As EventArgs) Handles btnProcessPendingStudents.Click
        If pendingNewStudentsQueue.Count = 0 Then
            MessageBox.Show("لا توجد طلاب معلقين لمعالجتهم.", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim processedStudent As student2 = pendingNewStudentsQueue.Dequeue() ' اسحب الطالب الأول من الطابور

        ' *** الآن يتم تسجيل الطالب رسمياً في النظام الرئيسي ***
        ' نستخدم nextStudentID الحالي للطالب المعالج
        processedStudent.StudentID = nextStudentID ' تعيين ID الجديد للطالب المعالج
        studentsData.Add(processedStudent)

        studentsDictionary.Add(processedStudent.StudentID, processedStudent)
        uniqueAddresses.Add(processedStudent.address)

        ' *** سجل عملية الإضافة هذه في مكدس التراجع ***
        Dim undoAction As New student2(student2.ActionType.AddStudent, processedStudent)
        undoStack.Push(undoAction)
        ' ****************************************************

        nextStudentID += 1 ' زيادة ID للطالب التالي، لأننا سجلنا طالباً الآن
        UpdateAddressComboBox() ' <---     ' إضافة العناوين من مجموعة uniqueAddresses إلى ComboBox

        MessageBox.Show($"تم تسجيل الطالب {processedStudent.name} (ID: {processedStudent.StudentID}) بنجاح من قائمة المعالجة المعلقة. يتبقى: {pendingNewStudentsQueue.Count} طلاب.", "معالجة طالب معلق", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ClearInputFields()
    End Sub
    'التلميح عند بدايت البحث على الاداة
    Private Sub TextBox_Search_TextChanged(sender As Object, e As EventArgs) Handles TextBox_Search.TextChanged
        Dim searchText As String = TextBox_Search.Text.Trim()
        ' إذا لم يتم إدخال شيء، عرض جميع الطلاب
        If String.IsNullOrWhiteSpace(searchText) Then
            StudentsDataGridView.DataSource = studentsData
            StudentsDataGridView.ClearSelection()
            ClearInputFields()
            Return
        End If
        ' البحث اللحظي بناءً على الاسم، العنوان، أو ID
        Dim searchResults As New BindingList(Of student2)()
        Dim searchID As Integer
        Dim isIDSearch As Boolean = Integer.TryParse(searchText, searchID)
        If isIDSearch AndAlso studentsDictionary.ContainsKey(searchID) Then
            searchResults.Add(studentsDictionary(searchID))
        Else
            For Each student As student2 In studentsData
                If student.name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 OrElse
               student.address.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 Then
                    searchResults.Add(student)
                End If
            Next
        End If
        StudentsDataGridView.DataSource = searchResults
        ' التعامل مع النتائج
        If searchResults.Count = 0 Then
            StudentsDataGridView.ClearSelection()
            ClearInputFields()
        Else
            StudentsDataGridView.ClearSelection()
            StudentsDataGridView.Rows(0).Selected = True
        End If
    End Sub
    ' إضافة معالج حدث لـ FormClosing من الفورم الاساسي 
    Private Sub Design_a_complete_program_for_students_DataGridView3_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        'استدعاء دالة الحفظ هنا
        SaveStudentsToCsv()
    End Sub
    Private Sub SaveDataButton_Click_2(sender As Object, e As EventArgs) Handles SaveDataButton.Click
        SaveStudentsToCsv()
    End Sub

    Private Sub LoadDataButton_Click_1(sender As Object, e As EventArgs) Handles LoadDataButton.Click
        LoadStudentsFromCsv()

    End Sub

End Class

