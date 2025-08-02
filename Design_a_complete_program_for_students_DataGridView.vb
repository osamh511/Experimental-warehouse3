Imports System.ComponentModel
Imports System.DirectoryServices
Imports System.Drawing.Text
Imports System.IO
Imports System.Security.Cryptography.X509Certificates

'students_DataGridViewتم استخدام الاداة هاذي بدل الاادةListBox
Public Class Design_a_complete_program_for_students_DataGridView
    '''   بشكل عام ملخص كود اظهار تسمية توضيحية للادوات، هذا الكود يقوم بما يلي:
    '''يخزن النصوص التوضيحية لكل مربع نص.
    '''يعرض النصوص التوضيحية باللون الرمادي عند تحميل النموذج.
    '''عندما يركز المستخدم على مربع نص يحتوي على النص التوضيحي، يتم مسح النص وتغيير لونه إلى الأسود.
    '''عندما يترك المستخدم مربع نص فارغًا، يتم إعادة عرض النص التوضيحي باللون الرمادي.
    '''
    ''' </summary>
    ' قاموس لتخزين النص التوضيحي لكل مربع نص
    'هذا القاموس سيقوم بتخزين العلاقة بين كل مربع نص (TextBox) والنص التوضيحي (Object - والذي سيكون نصًا من النوع String).
    Private placeholderTexts As New Dictionary(Of TextBox, Object) 'الهدف: هذا القاموس يوفر طريقة منظمة لربط كل مربع نص بالنص التوضيحي الخاص به.
    ' نص التلميح الافتراضي لكل مربع نص
    'كل ثابت يحمل النص التوضيحي الافتراضي لمربع نص معين (مثل اسم الطالب، العمر، العنوان، إلخ.

    '''المتغيرات والثوابت)
    '''Private placeholderTexts As New Dictionary(Of TextBox, Object): يتم هنا تعريف قاموس باسم placeholderTexts. هذا القاموس سيقوم بتخزين العلاقة بين كل مربع نص (TextBox) والنص التوضيحي (Object - والذي سيكون نصًا من النوع String).
    '''Private Const ... As String = "...": يتم هنا تعريف مجموعة من الثوابت (Const). كل ثابت يحمل النص التوضيحي الافتراضي لمربع نص معين (مثل اسم الطالب، العمر، العنوان، إلخ.). استخدام الثوابت يجعل الكود أسهل للقراءة والتعديل
    Private Const NamePlaceholder As String = "أدخل اسم الطالب"
    Private Const AgePlaceholder As String = "أدخل عمر الطالب (أرقام)"
    Private Const AddressPlaceholder As String = "أدخل عنوان الطالب"
    Private Const EnrollmentYearPlaceholder As String = "أدخل سنة الالتحاق (أرقام)"
    Private Const ClassPlaceholder As String = "أدخل الصف الدراسي (أرقام)"
    Private Const GradePlaceholder As String = "أدخل المعدل (أرقام)"
    Private Const SearchPlaceholder As String = "ابحث بالاسم أو الصف"
    Private Const MinAgePlaceholder As String = "الحد الأدنى للعمر"
    Private Const MaxAgePlaceholder As String = "الحد الأقصى للعمر"
    Private Const MinGradePlaceholder As String = "الحد الأدنى للمعدل"
    Private Const MaxGradePlaceholder As String = "الحد الأقصى للمعدل"

    '''    إجراء Design_a_complete_program_for_students_DataGridView_Load(يتم تنفيذه عند تحميل النموذج):
    '''تعبئة القاموس: يتم إضافة كل مربع نص من مربعات إدخال بيانات الطالب كمفتاح في القاموس placeholderTexts. والقيمة المرتبطة بكل مفتاح هي النص التوضيحي المناسب (المخزن في الثوابت).
    '''تطبيق النص التوضيحي الأولي: يتم المرور على جميع العناصر الموجودة في القاموس placeholderTexts. لكل مربع نص (المفتاح)، يتم تعيين خاصية Text الخاصة به بالنص التوضيحي المخزن كقيمة.
    '''تغيير لون الخط الأولي: في نفس الحلقة، يتم تعيين خاصية ForeColor (لون الخط) لكل مربع نص إلى اللون الرمادي (Color.Gray). هذا يجعل النص التوضيحي يظهر بلون باهت عند بداية تشغيل البرنامج.
    Private Sub Design_a_complete_program_for_students_DataGridView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '''العمل:  في هذه الأسطر، يتم إضافة أزواج المفاتيح والقيم إلى القاموس placeholderTexts.
        '''التركيز: لكل مربع نص (TextBox1، TextBox2، إلخ.)، يتم استخدامه كمفتاح، ويتم ربطه بالنص التوضيحي المناسب المخزن في الثابت المقابل (NamePlaceholder، AgePlaceholder، إلخ.) كقيمة.
        '''الهدف:  ملء القاموس بالمعلومات التي تربط كل مربع نص بالنص التوضيحي الذي يجب عرضه فيه
        ' تعيين النص التوضيحي الأولي وتخزينه في القاموس
        placeholderTexts.Add(TextBox1, NamePlaceholder)
        placeholderTexts.Add(TextBox2, AgePlaceholder)
        placeholderTexts.Add(TextBox3, AddressPlaceholder)
        placeholderTexts.Add(TextBox4, EnrollmentYearPlaceholder)
        placeholderTexts.Add(TextBox5, ClassPlaceholder)
        placeholderTexts.Add(TextBox6, GradePlaceholder)
        placeholderTexts.Add(TextBox7, SearchPlaceholder)
        placeholderTexts.Add(TextBox8, MinAgePlaceholder)
        placeholderTexts.Add(TextBox9, MaxAgePlaceholder)
        placeholderTexts.Add(TextBox10, MinGradePlaceholder)
        placeholderTexts.Add(TextBox11, MaxGradePlaceholder)
        ' تطبيق النص التوضيحي الأولي على مربعات النص
        '''تبدأ هنا حلقة تكرار (For Each). هذه الحلقة ستمر على كل عنصر في القاموس placeholderTexts.
        '''كل عنصر في القاموس يتم التعامل معه كـ KeyValuePair (زوج من المفتاح والقيمة)، ويتم تمثيله هنا بالمتغير kvp.
        For Each kvp In placeholderTexts 'KeyValuePair هي كلمة مختصرة لااسم الدورة
            '''العمل: داخل الحلقة،
            '''kvp.Key يشير إلى المفتاح في الزوج الحالي 
            '''(وهو كائن TextBox).
            '''kvp.Value 
            '''يشير إلى القيمة المرتبطة بهذا المفتاح
            '''(وهو النص التوضيحي)
            '''. يتم هنا تعيين خاصية
            '''Text
            '''الخاصة بمربع النص 
            '''(kvp.Key.Text) 
            '''إلى النص التوضيحي
            '''(kvp.Value).
            '''التركيز:    هذه الخطوة هي التي تجعل النص التوضيحي يظهر داخل كل مربع نص عند بدء تشغيل البرنامج.
            kvp.Key.Text = kvp.Value
            kvp.Key.ForeColor = Color.Gray
        Next
    End Sub
    '''    إجراء TextBox_Enter(يتم تنفيذه عند تركيز المستخدم على مربع نص):
    '''التحقق من وجود نص توضيحي: يتم التحقق أولاً ما إذا كان مربع النص الحالي (الذي تم التركيز عليه) موجودًا في القاموس placeholderTexts وما إذا كان النص الحالي الموجود فيه هو نفسه النص التوضيحي المخزن.
    '''مسح النص وتغيير اللون: إذا كان النص الحالي هو النص التوضيحي، يتم مسح محتوى مربع النص (currentTextBox.Text = "") وتغيير لون خطه إلى الأسود (currentTextBox.ForeColor = Color.Black) استعدادًا لكتابة المستخدم
    Private Sub TextBox_Enter(sender As Object, e As EventArgs) Handles TextBox1.Enter, TextBox2.Enter, TextBox3.Enter, TextBox4.Enter, TextBox5.Enter, TextBox6.Enter, TextBox7.Enter, TextBox8.Enter, TextBox9.Enter, TextBox10.Enter, TextBox11.Enter
        Dim currentTextBox As TextBox = DirectCast(sender, TextBox)        ' **يجب أن يكون placeholderTexts مرئيًا هنا**
        '''هذا التدليل البرمجي يوضح بالتفصيل عمل كل جزء من الشرط وكيف يؤدي تحققه إلى تنفيذ أمر مسح النص التوضيحي من مربع النص.
        '''إذا كان مربع النص الذي حصل على التركيز
        '''(currentTextBox)
        '''مسجلاً في قاموس النصوص التوضيحية
        '''(placeholderTexts)
        '''و كان النص الحالي المعروض في هذا المربع النص
        '''(currentTextBox.Text)
        '''مطابقًا تمامًا للنص التوضيحي الأصلي المخزن له في القاموس
        '''(placeholderTexts(currentTextBox))
        '''فقم بتنفيذ الأمر التالي: اجعل النص المعروض في مربع النص
        '''(currentTextBox.Text)
        '''سلسلة نصية فارغة ("")."

        '''خطوات التتبع البرمجي 
        ''' If placeholderTexts.ContainsKey(currentTextBox)
        ''' نعم
        ''' هذا الجزء يتحقق ما إذا كان currentTextBox
        ''' (مربع النص الحالي)
        ''' موجودًا كمفتاح في القاموس
        ''' placeholderTexts.
        ''' هذا الجزء يتحقق ما إذا كان النص الحالي 
        ''' (currentTextBox.Text)
        ''' مطابقًا للنص التوضيحي الأصلي المخزن في القاموس لهذا المربع النص 
        ''' placeholderTexts(currentTextBox)
        ''' 
        ''' واذى تحقق الشرط Treeفان 
        ''' currentTextBox.Text = "": 
        ''' نعم، إذا تحقق الشرط، يتم تعيين خاصية 
        ''' Text لمربع النص الحالي إلى سلسلة نصية فارغة، مما يؤدي إلى مسح النص التوضيحي.
        ''' 
        ''' ثم جعل لون خط التص المظلل لون اسود
        If placeholderTexts.ContainsKey(currentTextBox) AndAlso currentTextBox.Text = placeholderTexts(currentTextBox) Then
            '''شرح اذى تحقق الشرط بماذى يقوم الشرط 
            '''شرح الشرط كامل بدلالة برمجية 
            '''placeholderTexts(currentTextBox)
            '''هذه الخاصية
            '''(Property)
            '''تحتوي على النص الحالي الظاهر داخل مربع النص الذي حصل على التركيز.
            '''placeholderTexts(currentTextBox)
            ''': هنا نستخدم
            '''currentTextBox
            '''كمفتاح للوصول إلى القيمة المرتبطة به في القاموس
            '''placeholderTexts.
            '''هذه القيمة هي النص التوضيحي الأصلي الذي تم تعيينه لهذا المربع النص عند تحميل النموذج.
            ''' وعلامة = هي كانها تتحقق ما إذا كان النص الحالي في مربع النص مطابقًا تمامًا للنص التوضيحي الأصلي المخزن في القاموس.
            '''اذكان يساوي نفس النص التوضيحي الأصلي
            '''Text  قم بتعين خاصية
            '''الى سلسة نصية فارغة ""
            currentTextBox.Text = ""
            currentTextBox.ForeColor = Color.Black
        End If
    End Sub

    ''' إجراء TextBox_Leave (يتم تنفيذه عند فقدان مربع النص للتركيز):
    '''التحقق من أن المربع فارغ: يتم التحقق ما إذا كان مربع النص الحالي فارغًا (أو يحتوي على مسافات بيضاء فقط).
    '''إعادة تعيين النص التوضيحي وتغيير اللون: إذا كان مربع النص فارغًا، يتم إعادة تعيين النص التوضيحي الخاص به من القاموس (currentTextBox.Text = placeholderTexts(currentTextBox)) وتغيير لون خطه مرة أخرى إلى الرمادي (currentTextBox.ForeColor = Color.Gray).
    ''' 
    Private Sub TextBox_Leave(sender As Object, e As EventArgs) Handles TextBox1.Leave, TextBox2.Leave, TextBox3.Leave, TextBox4.Leave, TextBox5.Leave, TextBox6.Leave, TextBox7.Leave, TextBox8.Leave, TextBox9.Leave, TextBox10.Leave, TextBox11.Leave
        Dim currentTextBox As TextBox = DirectCast(sender, TextBox)
        '''إذا كان مربع النص موجودًا في القاموس وكان فارغًا عند فقدان التركيز، فإن هذا السطر يعيد عرض النص التوضيحي داخله.
        If placeholderTexts.ContainsKey(currentTextBox) AndAlso String.IsNullOrWhiteSpace(currentTextBox.Text) Then
            '''شرح اذى تحقق الشرط بماذى يقوم الشرط 
            '''تم هنا تعيين قيمة خاصية Text
            '''الخاصة بمربع النص الحالي
            '''(currentTextBox.Text)
            '''إلى القيمة المرتبطة به في القاموس
            '''placeholderTexts.
            '''هذه القيمة هي النص التوضيحي الأصلي.

            'التفسير في سياقنا: إذا كان مربع النص موجودًا في القاموس وكان فارغًا عند فقدان التركيز، فإن هذا السطر يعيد عرض النص التوضيحي داخله.
            currentTextBox.Text = placeholderTexts(currentTextBox)
            currentTextBox.ForeColor = Color.Gray ' أو أي لون رمادي فاتح تريده للنص التوضيحي
        End If
    End Sub

    Dim students_Data As New BindingList(Of student)
    Private EditingStudentIndex As Integer = -1 'متغير لتتبع الفهرس 
    ' إجراء لمسح محتويات مربعات الإدخال
    Private Sub ClearInputFields()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
    End Sub
    '    ا تحتاج إلى كتابته مرة أخرى إذا كان موجودًا في الكود الخاص بك. هذا الإجراء موجود بالفعل لتسهيل عملية تحديث عرض البيانات في االداة
    '    StudentsDataGridView كلما تغيرت قائمة students_Data (عند إضافة طالب جديد، أو حذف طالب، أو تعديل بيانات طالب).
    'تأكد فقط من أن لديك هذا الإجراء الفرعي في الكود الخاص بالفورم 

    ' إجراء لتحميل بيانات الطلاب إلى DataGridView
    Private Sub LoadStudentsToDataGridView()
        ' مسح أي مصدر بيانات سابق لـ DataGridView
        StudentsDataGridView.DataSource = Nothing
        ' ربط مصدر بيانات DataGridView بقائمة الطلاب
        StudentsDataGridView.DataSource = students_Data
        ''' DataGridViewسيعرض الآن البيانات الموجودة في 
        '''  كجدول منظم في .
        '''students_Data'
    End Sub

    ''   خطوات عمل زر "إضافة" (Button_Add_Click) في نقاط مختصرة:
    ''استخراج البيانات: يتم قراءة القيم المدخلة من مربعات النصوص الخاصة بالاسم، العمر، العنوان، سنة الالتحاق، الصف الدراسي، والمعدل.
    ''محاولة التحويل: يتم محاولة تحويل النصوص المدخلة للعمر، سنة الالتحاق، الصف الدراسي، والمعدل إلى أنواع البيانات الرقمية المناسبة (Integer و Double). يتم تخزين نتائج محاولة التحويل في متغيرات منطقية (isAgeValid, isEnrollmentYearValid, isGradeValid, issstudentClass).
    ''التحقق من صحة البيانات: يتم التحقق من أن جميع البيانات المطلوبة قد تم إدخالها وأن التحويلات الرقمية قد نجحت. بمعنى آخر، يتم التأكد من أن الاسم والعنوان ليسا فارغين وأن العمر وسنة الالتحاق والصف والمعدل تم إدخالها بشكل صحيح كأرقام.
    ''إنشاء كائن طالب جديد: إذا كانت جميع البيانات صحيحة، يتم إنشاء كائن جديد من الكلاس student.
    ''تعبئة بيانات الطالب: يتم تعيين قيم خصائص الكائن newStudent باستخدام البيانات المستخرجة من مربعات النصوص.
    ''إضافة الطالب إلى القائمة: يتم إضافة الكائن newStudent إلى قائمة students_Data التي تحتوي على جميع الطلاب.
    ''تحديث عرض الجدول: يتم استدعاء الإجراء LoadStudentsToDataGridView() لتحديث عرض البيانات في DataGridView وعرض الطالب الجديد المضاف.
    ''مسح مربعات الإدخال: يتم مسح محتويات جميع مربعات النصوص لتكون جاهزة لإدخال بيانات طالب جديد آخر.
    ''إظهار رسالة نجاح: تظهر رسالة تعلم المستخدم بنجاح عملية إضافة بيانات الطالب.
    Private Sub Button_Add_Click(sender As Object, e As EventArgs) Handles Button_Add.Click
        ' استخراج البيانات من مربعات النص
        Dim nname As String = TextBox1.Text.Trim()
        Dim aage As Integer
        Dim aaddress As String = TextBox3.Text.Trim()
        Dim eenrollmentYear As Integer
        Dim sstudentClass As Integer
        Dim ggrade As Double

        ' محاولة تحويل النصوص المدخلة إلى الأنواع الصحيحة
        Dim isAgeValid As Boolean = Integer.TryParse(TextBox2.Text.Trim(), aage)
        Dim isEnrollmentYearValid As Boolean = Integer.TryParse(TextBox4.Text.Trim(), eenrollmentYear)
        Dim isGradeValid As Boolean = Double.TryParse(TextBox6.Text.Trim(), ggrade)
        Dim issstudentClass As Boolean = Integer.TryParse(TextBox5.Text.Trim(), sstudentClass)

        ' التحقق من صحة البيانات المدخلة
        If Not String.IsNullOrEmpty(nname) AndAlso isAgeValid AndAlso Not String.IsNullOrEmpty(aaddress) AndAlso isEnrollmentYearValid AndAlso sstudentClass AndAlso isGradeValid Then
            ' إنشاء كائن طالب جديد
            Dim newStudent As New student() ' إضافته إلى قائمة students_Data باستخدام الأمر students_Data.Add(newStudent).
            newStudent.name = nname
            newStudent.age = aage
            newStudent.address = aaddress
            newStudent.enrollmentYear = eenrollmentYear
            newStudent.studentClass = sstudentClass
            newStudent.grade = ggrade
            ' إضافة الطالب الجديد إلى قائمة البيانات
            students_Data.Add(newStudent)
            ' تحديث عرض قائمة الطلاب باستخدام DataGridView بدلاً من ListBox
            LoadStudentsToDataGridView() ' <--- تم نقل هذا السطر إلى داخل كتلة If
            ' مسح مربعات الإدخال
            ClearInputFields()
            ' إظهار رسالة نجاح
            MsgBox("تمت اضافة بيانات الطالب بنجاح")
        End If
    End Sub
    ' عمل زر العرض 
    '1- استدعاء اجراء الفرعي الخاص ب(LoadStudentsToDataGridView)
    Private Sub Button_Show_Click(sender As Object, e As EventArgs) Handles Button_Show.Click
        ' تحميل بيانات الطلاب إلى DataGridView بدلاً من ListBox
        LoadStudentsToDataGridView() ' <--- تم التغيير لاستخدام DataGridView
        ' **الفرق:** الآن سيتم عرض البيانات في جدول منظم بدلاً من قائمة عمودية بسيطة.
        ' إظهار عدد الطلاب المعروضين
        MsgBox($"تم عرض الطلاب  {students_Data.Count}{vbCrLf}")
    End Sub

    '''خطوات عمل الحدث StudentsDataGridView_SelectionChanged في نقاط مختصرة:
    '''التحقق من وجود صف محدد: عند تغيير تحديد صف في DataGridView، يتأكد الكود أولاً من وجود صف واحد على الأقل محدد.
    '''الحصول على الصف المحدد: يتم الحصول على الكائن الذي يمثل الصف الأول المحدد في DataGridView.
    '''الحصول على كائن الطالب: يتم استخراج كائن student المرتبط بالصف المحدد.
    '''إنشاء تفاصيل الطالب كنص: يتم تنسيق معلومات الطالب (الاسم، العمر، العنوان، سنة الالتحاق، الصف الدراسي، المعدل) في سلسلة نصية واحدة.
    '''عرض التفاصيل في تلميح الأدوات: يتم استخدام الأداة StudentDetailsToolTip لعرض تفاصيل الطالب كنص عندما يقوم المستخدم بتمرير مؤشر الفأرة فوق أي مكان في DataGridView بعد تحديد الصف.
    '''عرض التفاصيل في مربع رسالة: يتم عرض نفس تفاصيل الطالب في مربع رسالة منفصل يظهر للمستخدم.
    '''باختصار، هذا الكود يستجيب لتغيير تحديد الصف في DataGridView عن طريق عرض معلومات الطالب المحدد بطريقتين: كتلميح يظهر عند تمرير الفأرة وكمربع رسالة منبثق.
    Private Sub StudentsDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles StudentsDataGridView.SelectionChanged ' <--- تم التغيير لاستخدام DataGridView
        ' التأكد من وجود صف محدد في DataGridView
        If StudentsDataGridView.SelectedRows.Count > 0 Then ' <--- تم التغيير لاستخدام DataGridView
            ''' الحصول على الصف المحدد (نفترض تحديد صف واحد فقط)
            '''لحصول على كائن الطالب المرتبط بالصف
            '''ذا السطر يجلب معلومات الطالب الكاملة من الصف الذي اخترته في الجدول (selectedRow) ويضعها في متغير اسمه selectedStudent
            ''' إنشاء رسالة تحتوي على قائمة معلومات الطالب'''ضع معلومات الطالب الكاملة للصف الذي تم اختياره في الجدول
            Dim selectedRow As DataGridViewRow = StudentsDataGridView.SelectedRows(0) ' <--- تم التغيير لاستخدام DataGridView
            ' الحصول على كائن الطالب المرتبط بالصف المحدد
            Dim selectedStudent As student = DirectCast(selectedRow.DataBoundItem, student) ' <--- تم التغيير لاستخدام DataGridView

            Dim studentDetails As String = $"name::{selectedStudent.name}" & vbCrLf &
                                              $"age: {selectedStudent.age}" & vbCrLf &
                                              $"address: {selectedStudent.address}" & vbCrLf &
                                              $"enrollmentYear: {selectedStudent.enrollmentYear}" & vbCrLf &
                                              $"studentClass: {selectedStudent.studentClass}" & vbCrLf &
                                              $"grade: {selectedStudent.grade}"

            '''هذا يعني أنه عند تمرير مؤشر الفأرة فوق أي مكان في DataGridView بعد تحديد صف، ستظهر تفاصيل الطالب في تلميح الأدوات
            StudentDetailsToolTip.SetToolTip(StudentsDataGridView, studentDetails)
            ' عرض معلومات الطالب في مربع حوار
            MessageBox.Show(studentDetails, $"تفاصيل الطالب: {selectedStudent.name}")
            ' **الفرق:** بدلاً من الحصول على عنصر نصي من ListBox، نحصل الآن على صف كامل من DataGridView
            ' ثم نستخرج كائن الطالب المرتبط بهذا الصف لعرض تفاصيله.
        End If
    End Sub

    '''خطوات عمل زر "حذف" (ButtonDelete_Click) في نقاط مختصرة:
    '''التحقق من وجود صف محدد: يتأكد الكود أولاً من أن هناك صفاً واحداً على الأقل محدداً في DataGridView.
    '''الحصول على فهرس الصف المحدد: يتم الحصول على فهرس (موقع) الصف الأول المحدد في DataGridView.
    '''الحصول على كائن الطالب: باستخدام الفهرس الذي تم الحصول عليه، يتم استرداد كائن student المقابل من قائمة students_Data. هذا هو الطالب الذي سيتم حذفه.
    '''عرض تأكيد الحذف: يظهر مربع حوار يسأل المستخدم عما إذا كان متأكداً من رغبته في حذف الطالب المحدد، مع عرض اسم الطالب للتأكيد.
    '''تنفيذ الحذف(إذا أكد المستخدم): إذا ضغط المستخدم على زر "نعم" في مربع الحوار:
    '''يتم حذف الطالب من قائمة students_Data باستخدام الفهرس المحفوظ.
    '''يتم تحديث عرض DataGridView لعكس عملية الحذف.
    '''في حال عدم تحديد طالب: إذا لم يتم تحديد أي صف في DataGridView عند الضغط على زر "حذف"، تظهر رسالة تنبيه للمستخدم لإعلامه بضرورة تحديد طالب أولاً.
    Private Sub ButtonDelete_Click(sender As Object, e As EventArgs) Handles ButtonDelete.Click
        ' التأكد من وجود صف محدد في DataGridView
        If StudentsDataGridView.SelectedRows.Count > 0 Then ' <--- تم التغيير لاستخدام DataGridView
            'إننا نصل إلى أول صف محدد في مجموعة SelectedRows باستخدام الفهرس 0.
            Dim selectedIndex As Integer = StudentsDataGridView.SelectedRows(0).Index ' <--- تم التغيير لاستخدام DataGridView
            ' الحصول على كائن الطالب المحدد
            Dim selectedStudent As student = students_Data(selectedIndex) 'الغرض: هذا السطر يحصل على كائن student الفعلي الذي يمثل الطالب الذي تم تحديده في DataGridView.
            ' عرض مربع تأكيد الحذف
            Dim result As DialogResult = MessageBox.Show($"هل أنت متأكد أنك تريد حذف الطالب: {selectedStudent.name}؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            ' إذا اختار المستخدم "نعم"
            If result = DialogResult.Yes Then
                ' إزالة الطالب من قائمة البيانات
                students_Data.RemoveAt(selectedIndex) 'تقوم بإزالة العنصر الموجود في الفهرس المحدد (selectedIndex).
                ' تحديث عرض DataGridView بدلاً من ListBox
                LoadStudentsToDataGridView() ' <--- تم التغيير لاستخدام DataGridView
                ' **الفرق:** لحذف عنصر من ListBox كنا نستخدم StudentsListBox1.SelectedIndex مباشرة.
                ' الآن نحصل على فهرس الصف المحدد في DataGridView لحذف العنصر المقابل من students_Data.
            End If
        Else
            ' عرض رسالة إذا لم يتم تحديد طالب للحذف
            MessageBox.Show("يرجى تحديد الطالب لحذفه من القائمة.", "تنبيه")
        End If
    End Sub

    '''    خطوات عمل زر "تعديل" (ButtonEdit_Click) في نقاط مختصرة:
    '''التحقق من وجود صف محدد: يتأكد الكود أولاً من أن هناك صفاً واحداً على الأقل محدداً في DataGridView.
    '''الحصول على الصف المحدد: يتم الحصول على الكائن الذي يمثل الصف الأول المحدد في DataGridView وتخزينه في المتغير selectedRow.
    '''الحصول على كائن الطالب: يتم استخراج كائن student المرتبط بالصف المحدد وتخزينه في المتغير selectedStudent. هذا الكائن يحتوي على بيانات الطالب الموجودة في ذلك الصف.
    '''ملء مربعات النص: يتم استخدام بيانات الطالب المستخرجة (selectedStudent) لملء مربعات النصوص الموجودة في الفورم (TextBox1 إلى TextBox6). هذا يسمح للمستخدم برؤية البيانات الحالية للطالب وتعديلها.
    '''تتبع فهرس الطالب للتعديل: يتم الحصول على فهرس الصف المحدد في DataGridView وتخزينه في المتغير EditingStudentIndex. هذا الفهرس مهم لتحديد أي طالب سيتم تحديث بياناته عند الضغط على زر "حفظ التعديل".
    '''في حال لم يتم تحديد طالب: إذا لم يتم تحديد أي صف في DataGridView عند الضغط على زر "تعديل"، تظهر رسالة تنبيه للمستخدم لإعلامه بضرورة تحديد طالب أولاً.
    Private Sub ButtonEdit_Click(sender As Object, e As EventArgs) Handles ButtonEdit.Click
        'للحصول على عدد الصفوف المحددة حاليًا. هذا ما نستخدمة حالياDataGridView
        If StudentsDataGridView.SelectedRows.Count > 0 Then ' <--- تم التغيير لاستخدام DataGridView
            '''الحصول على معلومات الطالب المرتبط بالصف
            '''التحقق من وجود صف محدد في الجدول
            '''selectedRow = (السطر الأول المحدد في StudentsDataGridView)
            '''  هذا السطر يحصل على الكائن الذي يمثل الصف المحدد في DataGridView ويخزنه في الاداة selectedRow
            Dim selectedRow As DataGridViewRow = StudentsDataGridView.SelectedRows(0) ' <--- تم التغيير لاستخدام DataGridView
            ''' الحصول على كائن الطالب المرتبط بالصف
            '''ضع معلومات الطالب الكاملة للصف الذي تم اختياره في الجدول (DataGridView) في المكان الذي جهزناه واسمه selectedStudent.
            ''' بكلمات أخرى، هذا السطر ببساطة يستخرج معلومات الطالب الكاملة من الصف الذي قمت بتحديده في الجدول ويجعلها متاحة للبرنامج للتعامل معها
            Dim selectedStudent As student = DirectCast(selectedRow.DataBoundItem, student) ' <--- تم التغيير لاستخدام DataGridView
            ' ملء مربعات النص ببيانات الطالب المحدد
            'بنان على المعلومات المتوفرة في خصائص الكلاس الفرعي
            TextBox1.Text = selectedStudent.name
            TextBox2.Text = selectedStudent.age
            TextBox3.Text = selectedStudent.address
            TextBox4.Text = selectedStudent.enrollmentYear
            TextBox5.Text = selectedStudent.studentClass
            TextBox6.Text = selectedStudent.grade

            '''تتبع فهرس الطالب المحدد للتعديل
            '''عندما تختار صفاً في جدول بيانات الطلاب (StudentsDataGridView) لتعديل معلوماته
            '''    ، يقوم هذا السطر من الكود بتسجيل رقم هذا الصف في المتغير EditingStudentIndex.
            '''    هذا الرقم سيُستخدم لاحقاً لتحديد الطالب الذي يجب تحديث بياناته في قائمة students_Data.
            ' EditingStudentIndex = (رقم الصف المحدد في StudentsDataGridView) (لتحديد أي طالب سيتم تعديله لاحقًا في القائمة الرئيسية)
            'SelectedRows(0)مـــلاحظـة
            'تعني عدد الصف المحدد في الاداة فقط
            'index= بينما رقم الفهرس يشير اين يقع الصف المحدد في الجدول او الاداة
            EditingStudentIndex = StudentsDataGridView.SelectedRows(0).Index ' <--- تم التغيير لاستخدام DataGridView

            ' **الفرق:** بدلاً من الحصول على فهرس من ListBox، نحصل الآن على فهرس الصف المحدد في DataGridView.
        Else
            ' عرض رسالة إذا لم يتم تحديد طالب للتعديل
            MessageBox.Show("يرجى تحديد طالب لتعديله من القائمة.", "تنبيه")
        End If
    End Sub
    '''  تلخيص كتلة الكود    
    '''    زر حفظ التعديل: عند الضغط، يقوم بما يلي:"
    '''"التحقق من وجود طالب قيد التعديل:"
    '''إذا (EditingStudentIndex أكبر من أو يساوي 0): (هل تم تحديد طالب مسبقًا للتعديل؟)
    '''"قراءة البيانات المُعدلة من مربعات النص:"
    '''name = (قيمة مدخلة في مربع نص الاسم . بدون فراغات)
    '''age = (قيمة مدخلة في مربع نص العمر . سيتم التحقق من أنها رقم)
    '''address = (قيمة مدخلة في مربع نص العنوان . بدون فراغات)
    '''enrollmentYear = (قيمة مدخلة في مربع نص سنة الالتحاق . سيتم التحقق من أنها رقم)
    '''studentClass = (قيمة مدخلة في مربع نص الصف الدراسي . سيتم التحقق من أنها رقم)
    '''grade = (قيمة مدخلة في مربع نص المعدل . سيتم التحقق من أنها رقم عشري)
    '''"التحقق من صحة البيانات المدخلة:"
    '''إذا (الاسم ليس فارغًا) و (العمر رقم صحيح) و (العنوان ليس فارغًا) و (سنة الالتحاق رقم صحيح) و (الصف الدراسي رقم صحيح) و (المعدل رقم عشري):
    '''"إنشاء طالب مُحدَّث:"
    '''updatedStudent = (كائن طالب جديد)
    '''updatedStudent.name = name
    '''updatedStudent.age = age
    '''updatedStudent.address = address
    '''updatedStudent.enrollmentYear = enrollmentYear
    '''updatedStudent.grade = grade
    '''updatedStudent.studentClass = studentClass
    '''"تحديث الطالب في القائمة الرئيسية:"
    '''students_Data (عند الفهرس EditingStudentIndex) = updatedStudent (استبدال الطالب القديم بالجديد في القائمة)
    '''"تحديث عرض الجدول:"
    '''LoadStudentsToDataGridView() (إعادة تحميل البيانات في الجدول لعرض التغييرات)
    '''"إظهار رسالة نجاح:"
    '''عرض رسالة: "تم تعديل بيانات الطالب بنجاح [اسم الطالب]."
    '''"إعادة تعيين حالة التعديل:"
    '''EditingStudentIndex = -1 (لم يعد هناك طالب قيد التعديل)
    '''"مسح مربعات الإدخال:"
    '''ClearInputFields() (تفريغ مربعات النص)
    '''إلا:
    '''"إظهار رسالة خطأ:"
    '''عرض رسالة: "يرجى إدخال جميع بيانات الطالب بشكل صحيح."
    '''إلا:
    '''"إظهار رسالة تنبيه:"
    '''عرض رسالة: "يرجى تحديد طالب لتعديله أولاً."

    '''    إليك خطوات عمل زر "حفظ التعديل" (ButtonSaveEdit_Click) في نقاط مختصرة:
    '''التحقق من وجود طالب قيد التعديل: يتأكد الكود أولاً أن هناك فهرس لطالب تم تحديده مسبقاً للتعديل (EditingStudentIndex >= 0).
    '''قراءة البيانات المُعدلة: إذا كان هناك طالب قيد التعديل، يتم قراءة القيم الجديدة التي أدخلها المستخدم في مربعات النصوص (الاسم، العمر، العنوان، سنة الالتحاق، الصف الدراسي، المعدل).
    '''التحقق من صحة البيانات: يتم التأكد من أن البيانات المدخلة صحيحة (الاسم ليس فارغاً، العمر وسنة الالتحاق والصف أرقام صحيحة، المعدل رقم).
    '''إنشاء كائن طالب مُحدَّث: إذا كانت البيانات صحيحة، يتم إنشاء كائن student جديد يحتوي على القيم المُعدلة.
    '''تحديث القائمة: يتم استبدال كائن الطالب القديم الموجود في قائمة students_Data بالكائن الجديد المُحدَّث باستخدام الفهرس المحفوظ (EditingStudentIndex).
    '''تحديث العرض: يتم تحديث عرض البيانات في DataGridView ليظهر التعديل الذي تم.
    '''إظهار رسالة نجاح: تظهر رسالة للمستخدم تعلمه بنجاح عملية التعديل.
    '''إعادة تعيين فهرس التعديل: يتم إعادة قيمة EditingStudentIndex إلى -1 للإشارة إلى أنه لا يوجد حالياً طالب قيد التعديل.
    '''مسح مربعات الإدخال: يتم مسح محتويات مربعات النصوص لتكون جاهزة لإدخال بيانات طالب جديد أو تعديل طالب آخر.
    '''في حال البيانات غير صحيحة: إذا كانت البيانات المدخلة غير صحيحة، تظهر رسالة خطأ للمستخدم لتصحيحها.
    '''في حال لم يتم تحديد طالب للتعديل: إذا لم يكن هناك طالب محدد للتعديل عند الضغط على الزر، تظهر رسالة تنبيه للمستخدم لتحديد طالب أولاً.
    Private Sub ButtonSaveEdit__Click(sender As Object, e As EventArgs) Handles ButtonSaveEdit_.Click
        '''هل يوجد طالب محدد للتعديل؟
        '''(EditingStudentIndex يخزن رقم الطالب الذي اخترناه سابقًا).
        ''' التأكد من وجود طالب قيد التعديل
        If EditingStudentIndex >= 0 Then 'اكبر من او يساوي
            ' قراءة القيم المُعدلة من مربعات النص
            Dim name As String = TextBox1.Text.Trim()
            Dim age As Integer
            Dim address As String = TextBox3.Text.Trim()
            Dim enrollmentYear As Integer
            Dim studentClass As Integer
            Dim grade As Double
            ' التحقق من صحة البيانات المُدخلة مع قواعد إضافية للنطاقات
            Dim isAgeValid As Boolean = Integer.TryParse(TextBox2.Text.Trim(), age) AndAlso age >= 5 AndAlso age <= 25
            Dim isEnrollmentYearValid As Boolean = Integer.TryParse(TextBox4.Text.Trim(), enrollmentYear) AndAlso enrollmentYear <= Date.Now.Year
            Dim isStudentClassValid As Boolean = Integer.TryParse(TextBox5.Text.Trim(), studentClass) AndAlso studentClass >= 1 AndAlso studentClass <= 12
            Dim isGradeValid As Boolean = Double.TryParse(TextBox6.Text.Trim(), grade) AndAlso grade >= 0 AndAlso grade <= 100

            ' إذا كانت جميع البيانات المدخلة صحيحة وفقًا لجميع القواعد
            If Not String.IsNullOrEmpty(name) AndAlso isAgeValid AndAlso Not String.IsNullOrEmpty(address) AndAlso isEnrollmentYearValid AndAlso isStudentClassValid AndAlso isGradeValid Then
                Dim updatedStudent As New student() ' اصنع نسخة طالب جديدة لتحمل البيانات المُعدلة.
                '''في هاذي الجزئية كامل كانحنا نقول 
                '''املاء جميع البيانات المعدلة في الاداةTextbox
                '''الى عناصر الكلاس الفرعي 
                updatedStudent.name = name
                updatedStudent.age = age
                updatedStudent.address = address
                updatedStudent.enrollmentYear = enrollmentYear
                updatedStudent.grade = grade
                updatedStudent.studentClass = studentClass

                '''استبدل الطالب القديم في قائمة الطلاب الرئيسية (students_Data)
                ''' تحديث الطالب الموجود في القائمة الرئيسية باستخدام الفهرس المحفوظ
                '''بالطالب الجديد المُعدل (باستخدام رقمه المخزن).
                students_Data(EditingStudentIndex) = updatedStudent
                ' تحديث عرض DataGridView بدلاً من ListBox
                LoadStudentsToDataGridView() 'إعادة تحميل البيانات في الجدول لعرض التغييرات
                ' عرض رسالة نجاح
                MessageBox.Show($" تعديل بيانات الطالب بنجاح {name}")
                ' إعادة تعيين حالة التعديل
                EditingStudentIndex = -1 'لم يعد هناك طالب قيد التعديل,انسى ان هناك طالب قيد التعديل الان
                ' مسح مربعات الإدخال
                ClearInputFields()
            Else ' إذا كانت هناك بيانات غير صحيحة
                ' إنشاء رسالة خطأ مفصلة لتوضيح المشاكل للمستخدم
                Dim errorMessage As String = "يرجى إدخال جميع بيانات الطالب بشكل صحيح:\n"
                If String.IsNullOrEmpty(name) Then
                    errorMessage &= "- الاسم مطلوب.\n"
                End If
                If Not isAgeValid Then
                    errorMessage &= "- العمر يجب أن يكون رقمًا صحيحًا بين 5 و 100.\n"
                End If
                If String.IsNullOrEmpty(address) Then
                    errorMessage &= "- العنوان مطلوب.\n"
                End If
                If Not isEnrollmentYearValid Then
                    errorMessage &= $"- سنة الالتحاق يجب أن تكون رقمًا صحيحًا ولا تتجاوز السنة الحالية ({Date.Now.Year}).\n"
                End If
                If Not isStudentClassValid Then
                    errorMessage &= "- الصف الدراسي يجب أن يكون رقمًا صحيحًا بين 1 و 12.\n"
                End If
                If Not isGradeValid Then
                    errorMessage &= "- المعدل يجب أن يكون رقمًا بين 0 و 100.\n"
                End If
                ' عرض رسالة خطأ إذا كانت البيانات غير صالحة
                MessageBox.Show(errorMessage, "خطاء في الادخال", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Else
            ' عرض رسالة إذا لم يتم تحديد طالب للتعديل
            MessageBox.Show("يرجى تحديد طالب لتعديله أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
    '"زر البحث: عند الضغط، يقوم بما يلي:"

    '''"تلخيص كتلة الكود للبحث الأساسي والمتقدم:"
    '''searchText = (قيمة مدخلة في مربع البحث الأساسي . بدون فراغات . حروف صغيرة)
    '''"تجهيز نتائج البحث وقرار التطابق:"
    '''searchResults = (قائمة فارغة سيتم ملؤها بالطلاب المطابقين)
    '''matchesSearchCriteria = (قرار مبدئي: الطالب يطابق)
    '''"جلب قيم البحث المتقدم (العمر والمعدل):"
    '''minAge, maxAge, minGrade, maxGrade = (قيم مدخلة في مربعات البحث المتقدم)
    '''isMinAgeValid, isMaxAgeValid, isMinGradeValid, isMaxGradeValid = (قرار: هل القيم المدخلة أرقام صحيحة/عشرية؟)
    '''"فحص كل طالب في القائمة:"
    '''لكل student في students_Data
    '''matchesSearchCriteria = (قرار جديد لكل طالب: يطابق)
    '''"فلترة البحث الأساسي (إذا وجد نص):"
    '''إذا(searchText ليس فارغًا) و (اسم الطالب لا يحتوي على searchText و صف الطالب لا يحتوي على searchText)
    '''matchesSearchCriteria = (قرار: لا يطابق)
    '''"فلترة البحث المتقدم (العمر إذا كانت القيم صحيحة):"
    '''إذا(isMinAgeValid) و (عمر الطالب < minAge)
    '''matchesSearchCriteria = (قرار: لا يطابق)
    '''إذا(isMaxAgeValid) و (عمر الطالب > maxAge)
    '''matchesSearchCriteria = (قرار: لا يطابق)
    '''"فلترة البحث المتقدم (المعدل إذا كانت القيم صحيحة):"
    '''إذا(isMinGradeValid) و (معدل الطالب < minGrade)
    '''matchesSearchCriteria = (قرار: لا يطابق)
    '''إذا(isMaxGradeValid) و (معدل الطالب > maxGrade)
    '''matchesSearchCriteria = (قرار: لا يطابق)
    '''"إضافة الطالب المطابق للنتائج:"
    '''إذا(matchesSearchCriteria هو نعم)
    '''        searchResults = searchResults + (الطالب الحالي)
    '''"عرض النتائج في الجدول وإظهار رسالة:"
    '''StudentsDataGridView.DataSource = searchResults(عرض الطلاب المطابقين في الجدول)
    '''        إذا(عدد الطلاب في searchResults يساوي 0)
    '''        إظهار رسالة:  "لم يتم العثور على أي طلاب يطابقون معايير البحث."

    '''        إظهار رسالة:  "تم العثور على [عدد الطلاب في searchResults] طالب يطابقون معايير البحث."
    '''     خطوات عمل زر "البحث" (ButtonSearch_Click) في نقاط مختصرة:
    '''الحصول على نص البحث: يتم استرداد النص الذي أدخله المستخدم في مربع البحث (TextBoxSearch8).
    '''تجهيز نص البحث: يتم حذف أي فراغات زائدة من بداية ونهاية النص وتحويل جميع الحروف إلى حروف صغيرة (ToLower()) لتسهيل عملية البحث غير الحساسة لحالة الأحرف.
    '''إنشاء قائمة لنتائج البحث: يتم إنشاء قائمة جديدة فارغة (searchResults) لتخزين الطلاب الذين تتطابق بياناتهم مع نص البحث.
    '''البحث في بيانات الطلاب: يتم المرور على كل طالب موجود في قائمة students_Data.
    '''التحقق من التطابق: لكل طالب، يتم التحقق مما إذا كان اسم الطالب (بعد تحويله إلى حروف صغيرة) يحتوي على نص البحث أو إذا كان الصف الدراسي للطالب (بعد تحويله إلى نص صغير) يحتوي على نص البحث.
    '''إضافة النتائج المتطابقة: إذا تطابق اسم الطالب أو صفه الدراسي مع نص البحث، يتم إضافة هذا الطالب إلى قائمة searchResults.
    '''عرض نتائج البحث في الجدول: يتم ربط عنصر DataGridView (StudentsDataGridView) بقائمة searchResults. هذا يؤدي إلى عرض الطلاب الذين تم العثور عليهم فقط في الجدول.
    '''إظهار رسالة بعد البحث:
    '''إذا كانت قائمة searchResults فارغة (لم يتم العثور على أي طلاب)، تظهر رسالة تعلم المستخدم بذلك.
    '''إذا تم العثور على طلاب، تظهر رسالة تعلم المستخدم بعدد الطلاب الذين تم العثور عليهم والكلمة التي تم البحث بها.
    Private Sub ButtonSearch_Click(sender As Object, e As EventArgs) Handles ButtonSearch.Click
        ' الحصول على نص البحث وتحويله إلى حروف صغيرة
        Dim searchText As String = TextBox7.Text.Trim().ToLower()
        '''هي نسخة مؤقتة من قائمة يمكنها حمل كائنات student.
        '''ستظل هذه القائمة موجودة طالما أن الإجراء ButtonSearch_Click قيد التنفيذ.
        Dim searchResults As New List(Of student) 'أنشئ قائمة فارغة لتخزين الطلاب الذين يطابقون البحث. (مثل سلة فارغة)
        Dim matchesSearchCriteria As Boolean = True 'فترض مبدئيًا أن كل طالب يطابق معايير البحث. (سنغير هذا إذا لم يطابق).

        ' الحصول على قيم البحث المتقدم
        Dim minAge As Integer
        Dim maxAge As Integer
        Dim minGrade As Double
        Dim maxGrade As Double
        ' محاولة تحويل قيم العمر المدخلة إلى أرقام
        Dim isMinAgeValid As Boolean = Integer.TryParse(TextBox8.Text.Trim(), minAge)
        Dim isMaxAgeValid As Boolean = Integer.TryParse(TextBox9.Text.Trim(), maxAge)
        ' محاولة تحويل قيم المعدل المدخلة إلى أرقام
        Dim isMinGradeValid As Boolean = Double.TryParse(TextBox10.Text.Trim(), minGrade)
        Dim isMaxGradeValid As Boolean = Double.TryParse(TextBox11.Text.Trim(), maxGrade)


        'ستحتوي searchResults على جميع الطلاب الذين تطابقوا مع البحث.
        For Each student As student In students_Data 'تصفح كل طالب في قائمة الطلاب الرئيسية.
            matchesSearchCriteria = True ' ابدأ من جديد بافتراض أن الطالب الحالي يطابق.

            ' التحقق من معيار البحث الأساسي (الاسم أو الصف) إذا تم إدخال نص بحث
            If Not String.IsNullOrEmpty(searchText) Then 'إذا كان هناك نص للبحث الأساسي
                'إذا كان اسم الطالب وصفه لا يحتويان على نص البحث (بعد تحويلهما لحروف صغيرة)، فالطالب لا يطابق
                If Not student.name.ToLower().Contains(searchText) AndAlso Not student.studentClass.ToString().ToLower().Contains(searchText) Then
                    matchesSearchCriteria = False
                End If
            End If
            '''إذا كان الحد الأدنى للعمر الذي أدخله المستخدم عددًا صحيحًا صالحًا، فتحقق بعد ذلك ما إذا كان عمر الطالب الحالي أقل من هذا الحد الأدنى. إذا كان كذلك،
            '''    فهذا يعني أن هذا الطالب لا يطابق معايير البحث الخاصة بالعمر، لذلك قم بتعيين علامة التطابق (matchesSearchCriteria) إلى False
            ''' التكرار على قائمة الطلاب والبحث عن تطابقات
            ''' 
            ' التحقق من معايير البحث المتقدم (العمر) إذا تم إدخال نطاق صحيح
            If isMinAgeValid Then 'إذا تم إدخال حد أدنى للعمر بشكل صحيح
                If student.age < minAge Then 'ذا كان عمر الطالب أقل من الحد الأدنى، فالطالب لا يطابق
                    matchesSearchCriteria = False
                End If
            End If
            If isMaxAgeValid Then
                If student.age > maxAge Then
                    matchesSearchCriteria = False
                End If
            End If

            ' التحقق من معايير البحث المتقدم (المعدل) إذا تم إدخال نطاق صحيح
            If isMinGradeValid Then
                If student.grade < minGrade Then
                    matchesSearchCriteria = False
                End If
            End If
            If isMaxGradeValid Then
                If student.grade > maxGrade Then
                    matchesSearchCriteria = False
                End If
            End If

            ' إذا تطابق الطالب مع جميع المعايير النشطة، نضيفه إلى قائمة النتائج
            ''' مـــــــــــــلاحـــظة
            '''            'شبيه بسيط:
            '''            تخيل أن searchResults هي سلة فارغة. في كل مرة نجد طالبًا يطابق معايير البحث، فإننا نضع هذا الطالب داخل السلة.
            '''بلغة الكود الفعلية، السطر المشابه لـ "searchResults = searchResults + (الطالب الحالي)" في VB.NET هو
            If matchesSearchCriteria Then
                searchResults.Add(student)
            End If
        Next

        ' ربط DataGridView بنتائج البحث
        'سيتم ربط DataGridView بهذه القائمة المؤقتة (searchResults) لعرض نتائج البحث في الجدول.
        StudentsDataGridView.DataSource = searchResults ' 

        ' عرض رسالة بناءً على نتائج البحث
        If searchResults.Count = 0 Then
            MessageBox.Show($"لم يتم العثور على أي طلاب يطابقون معايير البحث.", "نتائج البحث")
        Else
            MessageBox.Show($"تم العثور على {searchResults.Count} نتائـج ألـبحث", "طلاب يطابقون معاير البحث")
        End If
    End Sub

    'كود عملة يقوم بتعديل من خلال الجدول او الاداةStudentsDataGridView
    ''' <summary>
    '''          تلخيص كتلة الكود 
    '''  ''' استرداد البيانات المُعدلة: داخل معالج حدث CellEndEdit، سنقوم بما يلي:
    '''تحديد الصف والعمود الذي تم تعديله.
    '''استرداد القيمة الجديدة التي أدخلها المستخدم في الخلية.
    '''تحديد كائن student المقابل لهذا الصف في قائمة students_Data.
    '''تحديث الخاصية المناسبة لكائن student بالقيمة الجديدة.
    '''تحديث DataGridView: بعد تحديث كائن student في القائمة، سيتم تحديث عرض DataGridView تلقائياً لأنه مرتبط مباشرة بقائمة students_Data.
    ''' بااختصار بنقوم باالتالي
    '''       إذا كان العمود الذي تم تعديله هو هاذى (أشارة لااسم الخاصية)، فافعل كذاا
    ''' فافعل كاذا= (متغير الاول.الخاصية=تحويل.الى نص(بنائن على قيمة مدخلة.ماحذف المسافات  ))ي
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e">هذا يشير إلى خاصية RowIndex لكائن e الذي تم تمريره إلى معالج الحدثe</param>
    Private Sub StudentsDataGridView_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles StudentsDataGridView.CellEndEdit
        Try
            '''اختصار، هذا السطر من الكود يتحقق مما إذا كان التعديل الذي أدى إلى تفعيل حدث CellEndEdit قد حدث في خلية بيانات فعلية (صف وعمود صالحين)
            '''داخل DataGridView. إذا كان التعديل خارج نطاق بيانات الجدول (مثل محاولة التعديل في ترويسة الأعمدة أو الصفوف)
            '''، فلن يتم تنفيذ الكود الموجود داخل جملة If.
            'تأكد أن التعديل حدث في خلية بيانات حقيقية وليس في رأس الجدول
            ' التأكد من أن الصف والعمود الذي تم تعديله صالحان
            If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then 'تم استخدام علامة >= (أكبر من أو يساوي 0) للتحقق من فهرس الصف والعمود (e.RowIndex و e.ColumnIndex) لأنه في هياكل البيانات التي تعتمد على الفهرس مثل DataGridView، تبدأ الفهارس من الصفر وتزداد بشكل تصاعدي.
                '''                تعطيك الكائن الأصلي (من قائمتك students_Data) المرتبط بصف معين في الجدول.
                '''                يعني بااختصار "للحصول على بيانات الطالب الفعلية المرتبطة به في الكود الخاص بك"
                '''بما أن DataBoundItem يعيد كائنًا من النوع الأساسي Object، 
                '''فإننا نستخدم DirectCast
                '''لتحويله بشكل صريح إلى النوع الأكثر تحديدًا student. 
                '''هذا يسمح لنا بالوصول إلى خصائص كائن الطالب (مثل .name، .age).
                'احصل على معلومات الطالب الحقيقية المرتبطة بالصف الذي تم تعديله من قائمة الطلاب الرئيسية (students_Data).
                Dim editedStudent As student = DirectCast(StudentsDataGridView.Rows(e.RowIndex).DataBoundItem, student) 'يتم تخزين هذا الكائن student المسترد (مع جميع خصائصه الحالية) في متغير اسمه editedStudent.
                ''' مـــــــــــــــــــــلا حــــــظة()أي تغير 
                '''editedStudent اي تغير يتم على 
                '''students_Data سـيوثر على الكائن الموجود في القائمة الرئيسية

                ''' ببساطة، هذا السطر يساعد البرنامج على معرفة أي معلومة من معلومات الطالب (الاسم، العمر، العنوان، إلخ.)
                ''' قمت بتعديلها في الجدول، حتى يتمكن من تحديث الخاصية الصحيحة في كائن الطالب المخزن في الذاكرة.
                'اعرف اسم الخاصية (مثل "name" أو "age") المرتبطة بالعمود الذي تم تعديله.
                Dim columnName As String = StudentsDataGridView.Columns(e.ColumnIndex).DataPropertyName
                ' احصل على القيمة الجديدة التي أدخلها المستخدم في الخلية.
                Dim newValue As Object = StudentsDataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value 'هذا السطر يحصل على القيمة الجديدة التي أدخلها المستخدم في الخلية التي تم تعديلها.
                ''' ألشرط يتكلم باالشكل التالي ,تخيل أن هذه مثل جملة, إذا كان العمود الذي تم تعديله هو هاذى (أشارة لااسم الخاصية)، فافعل كذاا
                ''' تحديث خاصية الطالب بناءً على العمود الذي تم تعديله
                Select Case columnName
                    Case "name"  'هذا يعني "إذا كان اسم العمود(الخاصية)الذي تم تعديله هي'name'"
                        '''إذا تم تعديل عمود الاسم، فسيتم أخذ القيمة الجديدة التي أدخلتها (newValue)،
                        '''وتحويلها إلى نص، وإزالة أي فراغات زائدة منها، ثم تخزينها كاسم جديد للطالب
                        editedStudent.name = Convert.ToString(newValue).Trim() 'هاذى السطر تم استخدام كل المتغيرات مــلاحظة
                        MessageBox.Show("لقد تم تحديث بيانات الاسم")

                    Case "age" 'كما ذكرت سابقاً، هذه الحالة تُنفذ إذا كان المستخدم قد قام بتعديل عمود "age" (العمر).
                        Dim age As Integer
                        If Integer.TryParse(Convert.ToString(newValue).Trim(), age) Then
                            editedStudent.age = age ' اذى نجع التحويل حدث عمر الطالب 
                            MessageBox.Show("لقد تم تحديث بيانات العمر ")
                        Else
                            'اذى لم ينجح اظهر رسالة خطى
                            MessageBox.Show("يرجى ادخال قيمة عددية صحيحة ", "خطاء في الادخال", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            '''باختصار: هذا الجزء من الكود يفحص أي عمود في الجدول تم تعديله، ثم يأخذ القيمة الجديدة ويحاول تخزينها في الخاصية المناسبة لمعلومات الطالب في الذاكرة. في حالة إدخال عمر غير صحيح
                            '''، تظهر رسالة خطأ للمستخدم، ويتم إعادة تعيين قيمة الخلية إلى عمر الطالب القديم."
                            StudentsDataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = StudentsDataGridView.Rows(e.RowIndex).DataBoundItem.age
                        End If
                    Case "address"
                        editedStudent.address = Convert.ToString(newValue).Trim()
                        MessageBox.Show("لقد تم تحديث بيانات العنوان")
                    Case "enrollmentYear"
                        Dim enrollmentYear As Integer
                        MessageBox.Show("لقد تم تحديث بيانات سنة الالتحاق")
                        If Integer.TryParse(Convert.ToString(newValue).Trim(), enrollmentYear) Then
                            editedStudent.enrollmentYear = enrollmentYear
                        Else

                            MessageBox.Show("يرجى ادخال قيمة عددية صحيحة ", "خطاء في الادخال", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            StudentsDataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = StudentsDataGridView.Rows(e.RowIndex).DataBoundItem.enrollmentYear
                        End If
                    Case "studentClass"
                        Dim studentClass As Integer ' تم التغيير من Double إلى Integer
                        If Integer.TryParse(Convert.ToString(newValue).Trim(), studentClass) Then ' استخدام Integer.TryParse
                            editedStudent.studentClass = studentClass
                            MessageBox.Show("لقد تم تحديث بيانات الصف")
                        Else
                            MessageBox.Show("يرجى ادخال قيمة عددية صحيحة ", "خطاء في الادخال", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            StudentsDataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = StudentsDataGridView.Rows(e.RowIndex).DataBoundItem.studentClass
                        End If
                    Case "grade"
                        Dim grade As Double
                        If Double.TryParse(Convert.ToString(newValue).Trim(), grade) Then
                            editedStudent.grade = grade ' تم التغيير من editedStudent.studentClass = grade
                            MessageBox.Show("لقد تم تحديث بيانات المعدل/المجموع")
                        Else
                            MessageBox.Show("يرجى ادخال قيمة عددية صحيحة ", "خطاء في الادخال", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            StudentsDataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = StudentsDataGridView.Rows(e.RowIndex).DataBoundItem.grade
                        End If

                End Select 'نهاية التحقق من العمود الذي تم تعديله

            End If 'نهاية التحقق من أن التعديل كان في خلية بيانات صحيحة
        Catch ex As Exception 'إذا حدث أي خطأ أثناء التعديل
            MessageBox.Show($"حدث خطاء أثنا تعديل البيانات{ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
    '
    ''' <summary>
    '''             خطوات ملخصة لكود
    '''             ButtonSort_Click
    '''            
    '''
    '''زر "فرز": عند الضغط عليه، يقوم بما يلي:
    '''"تحديد عمود الفرز:"
    '''يحاول الحصول على اسم العمود الذي اختاره المستخدم من قائمة "فرز حسب".
    '''إذا لم يختر المستخدم شيئًا، يتجاهل الباقي ويظهر تنبيهًا.
    '''"تحديد اتجاه الفرز:"
    '''يفحص زر "تصاعدي". إذا كان محددًا، سيكون الفرز من الأصغر للأكبر.
    '''إذا لم يكن محددًا (بافتراض تحديد "تنازلي")، سيكون الفرز من الأكبر للأصغر.
    '''"تنفيذ الفرز حسب العمود المختار:"
    '''إذا اختار المستخدم "ألاسم": يرتب قائمة الطلاب أبجديًا حسب أسمائهم.
    '''إذا اختار المستخدم "العمر": يرتب قائمة الطلاب حسب أعمارهم (من الأصغر للأكبر افتراضيًا).
    '''إذا اختار المستخدم "المعدل": يرتب قائمة الطلاب حسب معدلاتهم (من الأصغر للأكبر افتراضيًا).
    '''إذا اختار المستخدم "سنة الالتحاق": يرتب قائمة الطلاب حسب سنوات التحاقهم (من الأقدم للأحدث افتراضيًا).
    '''إذا اختار المستخدم "الصف الدراسي": يرتب قائمة الطلاب حسب صفوفهم الدراسية (من الأصغر للأكبر افتراضيًا).
    '''ملاحظة: عملية الترتيب هذه تنشئ نسخة مرتبة جديدة من قائمة الطلاب.
    '''"عكس الترتيب إذا كان تنازليًا:"
    '''إذا تم تحديد خيار الفرز التنازلي، يتم عكس ترتيب القائمة التي تم فرزها للتو.
    '''"عرض البيانات المرتبة في الجدول:"
    '''يتم تحديث الجدول (StudentsDataGridView) ليُظهر قائمة الطلاب بعد تطبيق الفرز الجديد.
    '''"تنبيه إذا لم يتم تحديد عمود:"
    '''إذا لم يقم المستخدم باختيار أي عمود للفرز، تظهر له رسالة تنبيه تطلب منه تحديد عمود.
    '''                زر فرز بحسب
    '''            النتيجة النهائية للسطر

    '''عندما تكون قيمة 
    '''sortColumn هي "الاسم",
    '''يقوم هذا السطر بأخذ القائمة الرئيسية للطلاب
    '''(students_Data)
    '''كل طالب (بشكل تصاعدي افتراضي )وترتيبها بناءً على خاصية'name
    '''(students_Data) يقوم بتعيين هذه القائمة المرتبة حديثًا مرة أخرى إلى المتغير 
    ''' مما يؤدي إلى تحديث ترتيب الطلاب في الذاكرة
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <paeam sortColumn >اتجعل البرنامج يتوقف اذى لم يتحق الحصول على اي قيمة وارجع بكملة</paeam>
    ''' 


    ''' <summary>
    '''          طريقة اخرى الفرز بااستخدام BindingList
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub SortButton2_Click(sender As Object, e As EventArgs) Handles SortButton2.Click
        ''' مــــلاحظة
        '''(sortColumn)هو متغير يمثل نص بحيث يمثل اسم العمود مثل "ألاسم" أو "العمر"
        '''مـــلاحـظة علامة ؟ لاتجعل البرنامج يتوقف اذى لم يتحق الحصول على اي قيمة وارجع بكملة(Nothing)
        Dim sortColumn As String = SortByComboBox.SelectedItem?.ToString() 'اذى تم تحديد على عنصر من الاداة ارجع لنا بقيمتة
        If Not String.IsNullOrEmpty(sortColumn) Then ' التأكد من تحديد عمود للفرز
            '''متغير يحمل قيمة شرطية اذى تحقق ان المستخدم قام بتحديد على الاداة
            '''(SortAscendingRadioButton)وهي
            '''(حدد اتجاه الفرز بناءً على حالة, يااما تصاعدي او تنازلي RadioButtons.)
            Dim SortDirection As System.ComponentModel.ListSortDirection
            If SortAscendingRadioButton.Checked Then
                SortDirection = System.ComponentModel.ListSortDirection.Ascending
            Else
                SortDirection = System.ComponentModel.ListSortDirection.Descending
            End If

            Dim propertyName As String = ""

            Select Case sortColumn
                Case "ألاسم"
                    propertyName = "name"
                Case "العمر"
                    propertyName = "age"
                Case "المعدل"
                    propertyName = "grade"
                Case "سنة الالتحاق"
                    propertyName = "enrollmentYear"
                Case "الصف الدراسي"
                    propertyName = "studentClass"
            End Select

            If Not String.IsNullOrEmpty(propertyName) Then
                ' مسح أي عمليات فرز سابقة
                StudentsDataGridView.Sort(StudentsDataGridView.Columns(propertyName), SortDirection)
            Else
                MessageBox.Show("اسم العمود المحدد للفرز غير صالح.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Else
            MessageBox.Show("يرجى تحديد عمود الفرز من الاداة", "تـنـبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    'بشكل عام لتخزين معلومات طالب يطلب 
    '    بشكل مبسط:

    'SaveFileDialog هو النافذة التي تساعد المستخدم على اختيار مكان واسم ملف الحفظ.
    'WriteStudentDataToCsv هو الكود الذي يأخذ البيانات ويكتبها إلى الملف الذي اختاره المستخدم بتنسيق CSV.

    ''' <summary>
    '''           وظيفة الكود هي ::
    ''' الذي يقوم فعليًا بكتابة بيانات الطلاب إلى ملف CSV:
    '''تخيل: هذا الإجراء هو "العامل" الذي يقوم بالعمل الفعلي لكتابة المعلومات إلى الملف بعد أن يختار المستخدم المكان والاسم.
    '''وظيفته:
    '''فتح الملف: يقوم بفتح الملف الذي اختاره المستخدم ليتم الكتابة فيه.
    '''كتابة البيانات: يأخذ بيانات الطلاب الموجودة في برنامجك (بافتراض أنها معروضة في جدول DataGridView) ويكتبها إلى الملف سطرًا بسطر. كل معلومة للطالب (اسم، عمر، إلخ.) يتم فصلها بفواصل في كل سطر، وهذا هو تنسيق ملف CSV.
    '''ترتيب محدد: يكتب البيانات بنفس الترتيب الذي حددناه (الاسم أولاً، ثم العمر، وهكذا)، وهذا مهم حتى يتمكن البرنامج من قراءة البيانات بشكل صحيح عند التحميل لاحقًا.
    '''إغلاق الملف: بعد الانتهاء من الكتابة، يقوم بإغلاق الملف.
    '''في الكود: نستخدم كائنًا اسمه StreamWriter داخل هذا الإجراء لتسهيل عملية الكتابة إلى الملف. نتأكد من كتابة رؤوس الأعمدة أولاً ثم بيانات كل طالب في سطر منفصل.
    '''
    ''' </summary>
    ''' 
    ''' <param name="filePath"></param>

    ' إجراء فرعي لكتابة بيانات الطلاب إلى ملف CSV
    'بشكل برمجي: تعريف دالة فرعية لا ترجع قيمة وتستقبل معاملًا نصيًا.
    Private Sub WriteStudentDataToCsv(filePath As String) 'مـــلاحظة ذا الإجراء لا يُرجع أي قيمة.

        Try
            '''Using writer : هذه جملة Using تضمن أنه سيتم تحرير الموارد المستخدمة بواسطة الكائن writer تلقائيًا بمجرد الانتهاء من استخدامه (حتى لو حدث خطأ).
            '''هذا مهم جدًا عند التعامل مع الملفات لتجنب بقاء الملف مفتوحًا ومنع مشاكل أخرى.
            '''filePath
            '''هذا هو المسار الكامل للملف الذي سيتم الكتابة إليه (المسار الذي تم تمريره إلى الإجراء).
            '''False
            '''هذا المعامل الثاني يحدد ما إذا كنت تريد إلحاق النص بالملف الموجود (إذا كان موجودًا
            '''False
            '''يعني أننا سنقوم بكتابة محتوى جديد إلى الملف، وسيتم مسح أي محتوى سابق.
            '''إذا أردت إضافة البيانات إلى نهاية الملف، ستستخدم True.
            '''System.Text.Encoding.UTF8
            '''هذا المعامل الثالث يحدد ترميز الأحرف الذي سيتم استخدامه عند كتابة النص إلى الملف. UTF-8 هو ترميز شائع يدعم معظم الأحرف والرموز حول العالم، بما في ذلك اللغة العربية.

            'لوظيفة: إنشاء كائن
            'StreamWriter
            'لكتابة نص إلى الملف المحدد بالمسار (filePath)
            '. Using بشكل برمجي: إنشاء كائن لكتابة البيانات إلى الملف مع ضمان إدارة الموارد بشكل صحيح ثم اغلاقها
            '. False يعني الكتابة فوق أي محتوى موجود.
            'System.Text.Encoding.UTF8 يحدد ترميز النص.
            Using writer As New StreamWriter(filePath, False, System.Text.Encoding.UTF8) 'انشاء كائن يُستخدم لكتابة نص إلى ملف.
                'الوظيفة: كتابة سطر العناوين (أسماء الحقول) إلى الملف مفصولة بفواصل.
                'بشكل برمجي: استدعاء دالة لكتابة نص إلى الملف متبوعًا بسطر جديد.
                writer.WriteLine("الاسم,العمر,العنوان,سنة الالتحاق,الصف,المعدل")

                ' **تذكر: هنا تحتاج إلى الوصول إلى بيانات الطلاب المخزنة في برنامجك.**
                ' **بافتراض أن بيانات الطلاب معروضة في DataGridView باسم StudentsDataGridView:**
                ' مــــلاحظة
                ' (DataGridViewRow) هو عبارة عن نوع الموشر في الدورة 
                'بالفعل بمثابة متغير يعبر عن الصف الحالي في الدورة، ويستمد هذه الصفوف من المجموعة Rows
                'StudentsDataGridView.Rows.
                For Each row As DataGridViewRow In StudentsDataGridView.Rows 'الوظيفة: بدء حلقة تكرار للمرور على كل صف في جدول بيانات الطلاب (StudentsDataGridView).
                    If Not row.IsNewRow Then 'IsNewRow  لوظيفة: التحقق ما إذا كان الصف الحالي ليس هو الصف الجديد الفارغ في نهاية الجدول.

                        '''    ??بسبب هاذي العلامة احتجنا لااصدار احدث من فيجوال بيسك 
                        '''    هذه الأسطر تستخرج قيمة كل حقل من أعمدة الصف الحالي في DataGridView
                        '''    وتحولها إلى نص، مع التعامل مع القيم الفارغة بشكل آمن.
                        ''' Dim age As String = row.Cells("AgeColumn").Value?.ToString() ?? "" 'بشكل مبسط: "إذا كانت القيمة موجودة، حولها إلى نص. أما إذا كانت فارغة، فلا تفعل شيئًا (واترك النتيجة فارغة)."
                        '''Dim address As String = row.Cells("AddressColumn").Value?.ToString() ?? ""
                        '''Dim enrollmentYear As String = row.Cells("EnrollmentYearColumn").Value?.ToString() ?? ""
                        '''Dim classNumber As String = row.Cells("ClassColumn").Value?.ToString() ?? ""
                        '''Dim grade As String = row.Cells("GradeColumn").Value?.ToString() ?? ""

                        'شكل اخر للتعريف كود محول بدالة IF 
                        'Dim name As String

                        'If row.Cells("NameColumn").Value Is Nothing Then
                        '    name = ""
                        'Else
                        '    name = row.Cells("NameColumn").Value.ToString()
                        'End If

                        'شكل اخر للتعريف كود محول بدالة IF دالة الثلاثية 
                        Dim name As String = If(row.Cells("name").Value Is Nothing, "", row.Cells("name").Value.ToString())
                        Dim age As String = If(row.Cells("age").Value Is Nothing, "", row.Cells("age").Value.ToString())
                        Dim address As String = If(row.Cells("address").Value Is Nothing, "", row.Cells("address").Value.ToString())
                        Dim enrollmentYear As String = If(row.Cells("enrollmentYear").Value Is Nothing, "", row.Cells("enrollmentYear").Value.ToString())
                        Dim studentClass As String = If(row.Cells("studentClass").Value Is Nothing, "", row.Cells("studentClass").Value.ToString())
                        Dim grade As String = If(row.Cells("grade").Value Is Nothing, "", row.Cells("grade").Value.ToString())
                        ' الوظيفة: كتابة بيانات الطالب الحالي (المستخرجة من الصف) إلى الملف في سطر واحد مفصولة بفواصل.
                        'بشكل برمجي: كتابة نص منسق باستخدام استيفاء السلسلة.
                        writer.WriteLine($"{name},{age},{address},{enrollmentYear},{studentClass},{grade}")
                    End If
                Next 'ينتقل إلى الصف التالي في الحلقة( StudentsDataGridView).

            End Using 'يُنهي جملة Using writer. سيتم إغلاق الملف تلقائيًا هنا.

            'إذا وصلت التعليمات البرمجية إلى هنا بدون حدوث أي أخطاء في كتلة Try, فسيتم عرض رسالة للمستخدم
            '(MessageBox) لتأكيد أن عملية الحفظ قد تمت بنجاح.
            MessageBox.Show("تم حفظ بيانات الطلاب بنجاح.", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء حفظ البيانات: {ex.Message}", "خطأ في الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    ''' <summary>
    '''            وظيفة الكود التالي ::
    ''' إنشاء مربع حوار لحفظ الملف (SaveFileDialog):
    '''تخيل: هو مثل النافذة الصغيرة التي تظهر لك عندما تضغط على "حفظ باسم" في أي برنامج (مثل Word أو الرسام).
    '''وظيفته: يسمح للمستخدم(الشخص الذي يستخدم برنامجك) بتحديد:
    '''مكان حفظ الملف: أي مجلد في جهاز الكمبيوتر يريد حفظ الملف فيه.
    '''اسم الملف: الاسم الذي سيُطلق على الملف (مثل "بيانات الطلاب").
    '''نوع الملف: التنسيق الذي سيتم به حفظ الملف (في حالتنا، ملف CSV).
    '''في الكود: نستخدم كائنًا جاهزًا اسمه SaveFileDialog لإنشاء وعرض هذه النافذة للمستخدم داخل برنامجنا. الكود الذي كتبناه يضبط بعض خصائص هذه النافذة مثل العنوان، وأنواع الملفات المقترحة (CSV وكل الملفات)، والاسم الافتراضي للملف.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub SaveDataButton_Click(sender As Object, e As EventArgs) Handles SaveDataButton.Click
        LoadStudentsToDataGridView()
        'الصنف (هو الكلاس الاساسي )هو مثل قالب أو مخطط جاهز لكيفية عمل نافذة حفظ الملف
        '. New تقوم بإنشاء نسخة حقيقية من هذا القالب في الذاكرة. والآن، المتغير
        'saveFileDialog يشير إلى هذه النسخة.
        'والي راح يحمل البيانات 
        Dim saveFileDialog As New SaveFileDialog() 'هو قالب جاهز أو نافذة قياسية يوفرها نظام التشغيل (مثل نافذة "حفظ باسم" التي تراها في معظم البرامج). هذا الكائن يسمح لبرنامجك بعرض هذه النافذة للمستخدم ليختار مكان حفظ ملف واسمه.
        'بااختصار يخبر المستخدم أي أنواع من الملفات يجب أن تعرضها كخيارات للمستخدم.
        saveFileDialog.Filter = "ملفات CSV (*.csv)|*.csv|جميع الملفات (*.*)|*.*" ' نقوم بتعيين قيمة نصية إلى خاصية Filter للكائن saveFileDialog. هذه القيمة تخبر نافذة الحفظ أي أنواع من الملفات يجب أن تعرضها كخيارات للمستخدم.
        saveFileDialog.Title = "حفظ بيانات الطلاب" ' هذه الخاصية تحدد العنوان الذي سيظهر في الشريط العلوي لنافذة "حفظ باسم

        '''هذه الخاصية تحدد "الامتداد الافتراضي" للملف إذا لم يقم المستخدم بإدخال امتداد بنفسه.
        '''بشكل برمجي: نصل إلى خاصية DefaultExt التابعة للكائن saveFileDialog.
        saveFileDialog.DefaultExt = "csv"        ' نقوم بتعيين قيمة نصية "csv" إلى خاصية DefaultExt.
        '''     FileName تحدد "اسم الملف الافتراضي" الذي سيظهر في مربع النص الخاص باسم الملف عند فتح النافذة
        '''
        '''= "StudentsData.csv":
        '''تقوم بشكل افتراضي  بتعيين قيمة نصية "StudentsData.csv"
        '''إلى خاصية FileName

        saveFileDialog.FileName = "StudentsData.csv" ' تحدد "اسم الملف الافتراضي" الذي سيظهر في مربع النص الخاص باسم الملف عند فتح النافذة

        ' عرض مربع الحوار والسماح للمستخدم باختيار مكان الحفظ
        '''الوظيفة: هذا الجزء يعرض نافذة "حفظ باسم" للمستخدم وينتظر حتى يغلقها.
        '''بشكل برمجي: نستدعي "دالة"(Method) تابعة للكائن saveFileDialog 
        '''اسمها ShowDialog().
        ''' هذه الدالة تعرض النافذة وترجع "نتيجة" (DialogResult) عندما يغلقها المستخدم
        If saveFileDialog.ShowDialog() = DialogResult.OK Then '  هذا الجزء يعرض نافذة "حفظ باسم" للمستخدم وينتظر حتى يغلقه
            'الوظيفة: إذا ضغط المستخدم على "حفظ"، هذا السطر ينشئ متغيرًا اسمه filePath
            'ويخزن فيه "المسار الكامل للملف" الذي اختاره المستخدم (مكان الحفظ واسم الملف).
            Dim filePath As String = saveFileDialog.FileName
            ' استدعاء الإجراء الذي يقوم فعليًا بكتابة البيانات إلى الملف
            '''لوظيفة:هذا السطر يستدعي الإجراء الفرعي الآخر 
            '''(WriteStudentDataToCsv)
            '''ويبعث إليه المسار الكامل للملف
            '''(filePath)
            '''حتى يتمكن هذا الإجراء من كتابة البيانات إلى الملف الصحيح.
            '''بشكل برمجي: نقوم باستدعاء دالة أخرى اسمها 
            '''WriteStudentDataToCsv
            '''ونمرر لها قيمة المتغير filePath
            '''كـ "معامل" (Argument).
            WriteStudentDataToCsv(filePath)
        End If

    End Sub
    ''' <summary>
    '''        حليل جزئية جزئية بطريقة  Algorithms:
    '''يمكن تمثيل هذه الجزئية من الكود بخوارزمية بسيطة كالتالي:
    '''عرض نافذة "فتح الملف" للمستخدم.
    '''التحقق من نتيجة تفاعل المستخدم مع النافذة:
    '''إذا كانت النتيجة هي "تم الضغط على زر 'فتح'" (DialogResult.OK):
    '''احصل على مسار الملف الذي اختاره المستخدم من نافذة "فتح الملف".
    '''خزن هذا المسار في المتغير المسمى filePath.
    '''إذا كانت النتيجة ليست "تم الضغط على زر 'فتح'" (أي نتيجة أخرى):
    '''خزن قيمة فارغة في المتغير المسمى filePath.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>

    'بشكل مبسط
    'هذا السطر من الكود يقوم بفتح نافذة اختيار الملف للمستخدم. إذا اختار المستخدم ملفًا وضغط على "فتح"، فسيتم تخزين مسار هذا الملف في المتغير filePath. أما إذا ألغى المستخدم العملية أو أغلق النافذة بدون اختيار ملف، فسيتم تخزين قيمة فارغة في filePath
    Private Sub LoadDataButton_Click(sender As Object, e As EventArgs) Handles LoadDataButton.Click
        ' هذا المتغير سيُستخدم لتخزين مسار الملف الذي سيتم فتحه
        Dim openFileDialog As New OpenFileDialog()
        'openFileDialog.Filter 
        'هذه الخاصية تحدد أنواع الملفات التي ستظهر في قائمة التصفية في نافذة "فتح"
        openFileDialog.Filter = "ملفات CSV (*.csv)|*.csv|جميع الملفات (*.*)|*.*" 'هذه القيمة هي سلسلة نصية تحدد خيارين لتصفية الملفات (بنفس طريقة SaveFileDialog)
        openFileDialog.Title = "تحميل بيانات الطلاب"
        openFileDialog.DefaultExt = "csv"
        openFileDialog.FileName = "StudentsData.csv"

        '''       IF  دالة ثلاثية 
        '''بالتأكيد، سأقوم بتحويل جزئية الكود باستخدام جملة If...Then إلى تعبير دالة If الثلاثية.
        '''الكود الأصلي(باستخدام If...Then)
        '''مقتطف الرمز
        '''If openFileDialog.ShowDialog() = DialogResult.OK Then
        '''            Dim filePath As String = openFileDialog.FileName
        '''            ReadStudentDataFromCsv(filePath)
        '''        End If
        '''        الكود المحول إلى دالة If الثلاثية (في سطر واحد)
        '''مقتطف الرمز
        '''Dim filePath As String = If(openFileDialog.ShowDialog() = DialogResult.OK, openFileDialog.FileName, "")
        '''        If filePath <> "" Then ReadStudentDataFromCsv(filePath)
        '''        شرح الكود المحول
        '''Dim filePath As String = If(openFileDialog.ShowDialog() = DialogResult.OK, openFileDialog.FileName, "")
        '''        If (openFileDialog.ShowDialog() = DialogResult.OK, ... , ...) Then :
        '''            هذه هي دالة If الثلاثية.
        '''openFileDialog.ShowDialog() = DialogResult.OK : هذا هو الشرط. يتم عرض مربع حوار الفتح، ويتم التحقق مما إذا كانت النتيجة هي DialogResult.OK (أي ضغط المستخدم على "فتح").
        '''openFileDialog.FileName : هذا هو القيمة التي سيتم إرجاعها إذا كان الشرط صحيحًا. إذا ضغط المستخدم على "فتح"، فسيتم تخزين مسار الملف الذي اختاره في المتغير filePath.
        '''"": هذا هو القيمة التي سيتم إرجاعها إذا كان الشرط خاطئًا. إذا ضغط المستخدم على "إلغاء الأمر"، فسيتم تعيين سلسلة نصية فارغة ("") للمتغير filePath.

        'راح نستخدم دالة شرطية ثلاثية IF
        'اذى كان الشرط صحيح(Try)اذكان الشرط صحيح 
        'openFileDialog.FileName راح ترجع قيمة
        'الي بدورة تحتوي على المسار الكامل للملف
        '(fals)واذى كان خاطى
        'راح ترجع قيمة نصية فارغة الي علامتة("")ة 
        Dim filePath As String = If(openFileDialog.ShowDialog() = DialogResult.OK, openFileDialog.FileName, "")
        If filePath <> "" Then
            ' يتم تمرير قيمة المتغير بحيث داخل الاجراء  سيتم استخدام هذا المسار لقراءة محتويات الملف. filePath كـ معامل (argument) إلى هذا الإجراء
            ReadStudentDataFromCsv(filePath) ' هو المسؤول عن قراءة بيانات الطلاب من ملف CSV
        End If
        'بعد تعيين قيمة filePath باستخدام الدالة الثلاثية، نتحقق مما إذا كانت filePath ليست سلسلة نصية فارغة (""). هذا يعني أن المستخدم قد ضغط على "فتح" واختار ملفًا.
        'إذا كان filePath يحتوي على مسار ملف، فسيتم استدعاء الإجراء ReadStudentDataFromCsv مع هذا المسار لتحميل البيانات.
        'If openFileDialog.ShowDialog() = DialogResult.OK Then
        '        Dim filePath As String = openFileDialog.FileName
        '        ' استدعاء الإجراء الذي يقوم فعليًا بقراءة البيانات من الملف
        '        ReadStudentDataFromCsv(filePath)
        '    End If


        ' عرض مربع الحوار والسماح للمستخدم باختيار الملف

    End Sub
    ''' <summary>
    '''           سيكون هذا الإجراء مسؤولاً عن:
    '''استقبال مسار الملف CSV كمعامل.
    '''فتح الملف للقراءة باستخدام StreamReader.
    '''قراءة الملف سطرًا بسطر.
    '''تقسيم كل سطر إلى حقول بناءً على الفاصل (عادةً فاصلة ",").
    '''التحقق من عدد الحقول في كل سطر للتأكد من مطابقته لتوقعاتنا (اسم، عمر، عنوان، سنة الالتحاق، الصف الدراسي، المعدل - أي 6 حقول).
    '''إنشاء كائن جديد من نوع student لكل سطر صالح.
    '''تعيين قيم خصائص الكائن student من الحقول المقروءة.
    '''إضافة الكائن student الجديد إلى قائمة students_Data.
    '''معالجة أي أخطاء محتملة أثناء قراءة الملف (مثل تنسيق غير صالح).
    '''    ''' </summary>
    '''    

    Private Sub ReadStudentDataFromCsv(filePath As String)
        Try
            ' تأكد من أن students_Data هو BindingList(Of student)
            students_Data.Clear() ' مسح أي بيانات موجودة

            Using reader As New StreamReader(filePath, System.Text.Encoding.UTF8)
                ' قراءة سطر العناوين وتجاهله إذا كان موجودًا
                If Not reader.EndOfStream Then
                    reader.ReadLine() ' يمكنك التعليق على هذا السطر إذا لم يكن ملف CSV يحتوي على رؤوس
                End If

                While Not reader.EndOfStream
                    Dim line As String = reader.ReadLine()
                    Dim fields As String() = line.Split(",")

                    ' التحقق من أن السطر يحتوي على العدد المتوقع من الحقول
                    If fields.Length = 6 Then
                        Dim newStudent As New student()
                        newStudent.name = fields(0).Trim()
                        Dim age As Integer
                        If Integer.TryParse(fields(1).Trim(), age) Then
                            newStudent.age = age
                        Else
                            MessageBox.Show($"تنسيق عمر غير صالح: {fields(1)} في السطر: {line}", "خطأ في التحميل", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Continue While ' الانتقال إلى السطر التالي
                        End If
                        newStudent.address = fields(2).Trim()
                        Dim enrollmentYear As Integer
                        If Integer.TryParse(fields(3).Trim(), enrollmentYear) Then
                            newStudent.enrollmentYear = enrollmentYear
                        Else
                            MessageBox.Show($"تنسيق سنة الالتحاق غير صالح: {fields(3)} في السطر: {line}", "خطأ في التحميل", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Continue While
                        End If
                        Dim studentClass As Integer
                        If Integer.TryParse(fields(4).Trim(), studentClass) Then
                            newStudent.studentClass = studentClass
                        Else
                            MessageBox.Show($"تنسيق الصف الدراسي غير صالح: {fields(4)} في السطر: {line}", "خطأ في التحميل", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Continue While
                        End If
                        Dim grade As Double
                        If Double.TryParse(fields(5).Trim(), grade) Then
                            newStudent.grade = grade
                        Else
                            MessageBox.Show($"تنسيق المعدل غير صالح: {fields(5)} في السطر: {line}", "خطأ في التحميل", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Continue While
                        End If

                        students_Data.Add(newStudent) ' إضافة الطالب إلى BindingList
                    Else
                        MessageBox.Show($"تنسيق سطر غير صالح (يجب أن يحتوي على 6 حقول): {line}", "خطأ في التحميل", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                End While

            End Using

            MessageBox.Show($"تم استيراد {students_Data.Count} طالبًا بنجاح.", "تم الاستيراد", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء قراءة الملف: {ex.Message}", "خطأ في التحميل", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    ''' <summary>
    '''           عندما ينقر المستخدم على زر         
    ''' "استيراد"، سيتم تنفيذ هذا المعالج. سيقوم هذا المعالج بما يلي:
    '''إنشاء كائن من نوع OpenFileDialog للسماح للمستخدم باختيار ملف CSV.
    '''تكوين خصائص OpenFileDialog (مثل تحديد نوع الملفات المسموح بها، العنوان، إلخ.).
    '''عرض مربع حوار فتح الملف للمستخدم.
    '''إذا اختار المستخدم ملفًا وضغط على "فتح"، فسيتم الحصول على مسار الملف.
    '''استدعاء إجراء فرعي جديد (سنقوم بإنشائه في الخطوة التالية) لقراءة البيانات من الملف المحدد وتعبئة قائمة students_Data (التي ستحولها إلى BindingList كما ناقشنا).
    '''(بما أننا سنستخدم BindingList) سيتم تحديث StudentsDataGridView تلقائيًا بعد تعبئة القائمة.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ButtonImport_Click(sender As Object, e As EventArgs) Handles ButtonImport.Click
        Dim openFileDialog As New OpenFileDialog()

        ' تكوين خصائص مربع حوار فتح الملف
        openFileDialog.Title = "استيراد بيانات الطلاب من ملف CSV"
        openFileDialog.Filter = "ملفات CSV (*.csv)|*.csv|جميع الملفات (*.*)|*.*"
        openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)

        ' عرض مربع حوار فتح الملف
        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Dim filePath As String = openFileDialog.FileName
            ' استدعاء الإجراء الفرعي لقراءة البيانات من الملف
            ReadStudentDataFromCsv(filePath)
        End If
    End Sub
End Class

