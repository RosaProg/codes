package deliverytrack.vss.com.deliverytrack.utility.pdfwriter;

import android.os.Environment;
import android.util.Log;

import com.itextpdf.text.BadElementException;
import com.itextpdf.text.Document;
import com.itextpdf.text.DocumentException;
import com.itextpdf.text.Element;
import com.itextpdf.text.Font;
import com.itextpdf.text.Paragraph;
import com.itextpdf.text.Phrase;
import com.itextpdf.text.pdf.PdfPCell;
import com.itextpdf.text.pdf.PdfPTable;
import com.itextpdf.text.pdf.PdfWriter;
import com.parse.ParseUser;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.OutputStream;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;

import deliverytrack.vss.com.deliverytrack.models.OrderTransaction;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;

/**
 * Created by Adi-Loch on 1/22/2016.
 */
public class PDFUtils {





    private static Font catFont = new Font(Font.FontFamily.TIMES_ROMAN, 18,
            Font.BOLD);


    public static File createPDF(List<OrderTransaction> orderTransactions, double sum) {

        File pdfFolder = new File(Environment.getExternalStorageDirectory(), "bills");
        if (!pdfFolder.exists()) {
            pdfFolder.mkdir();
            Log.i("Tag", "Pdf Directory created");
        }
        //Create time stamp
        Date date = new Date();
        String timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss").format(date);
        File myFile = new File(pdfFolder + timeStamp + ".pdf");
        OutputStream output = null;
        try {
            output = new FileOutputStream(myFile);
            Document document = new Document();
            try {
                PdfWriter.getInstance(document, output);
            } catch (DocumentException e) {
                e.printStackTrace();
            }
            document.open();
            try {
                addTitlePage(document);
                String text = "";
                String id = "";

                if (DeliveryTrackUtils.isRestaurentUser()) {
                    text = "Restaurant Name" + " : " + ParseUser.getCurrentUser().getUsername();
                    id = "Restaurant Id" + " : " + ParseUser.getCurrentUser().getObjectId();

                } else if (DeliveryTrackUtils.isAgent()) {
                    text = "Delivery Agent Name" + " : " + ParseUser.getCurrentUser().getUsername();
                    id = "Delivery Agent Id" + " : " + ParseUser.getCurrentUser().getObjectId();


                }

                String address =
                        ParseUser.getCurrentUser().get("address") != null ?
                                ParseUser.getCurrentUser().get("address").toString() : "";
                String addressText = "Address :" + address;
                document.add(new Paragraph(text));
                document.add(new Paragraph(id));
                document.add(new Paragraph(addressText));
                createTable(document, orderTransactions);


            } catch (DocumentException e) {
                e.printStackTrace();
            }


            document.close();

             return myFile;
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        }

        return null;
    }



    private static void createTable(Document document, List<OrderTransaction> orderTransactions)
            throws BadElementException {
        PdfPTable table = new PdfPTable(9);
        table.setWidthPercentage(80);
        try {
            table.setWidths(new int[]{50, 200, 200, 200, 200, 200, 200, 200, 200});
        } catch (DocumentException e) {
            e.printStackTrace();
        }
        PdfPCell c1 = new PdfPCell(new Phrase("S.No"));
        c1.setHorizontalAlignment(Element.ALIGN_JUSTIFIED);
        table.addCell(c1);

        c1 = new PdfPCell(new Phrase("Order Date"));
        c1.setHorizontalAlignment(Element.ALIGN_JUSTIFIED);
        table.addCell(c1);

        c1 = new PdfPCell(new Phrase("Order No"));
        c1.setHorizontalAlignment(Element.ALIGN_JUSTIFIED);
        table.addCell(c1);


        c1 = new PdfPCell(new Phrase("Restaurant Name"));
        c1.setHorizontalAlignment(Element.ALIGN_JUSTIFIED);
        table.addCell(c1);


        c1 = new PdfPCell(new Phrase("Location"));
        c1.setHorizontalAlignment(Element.ALIGN_JUSTIFIED);
        table.addCell(c1);


        c1 = new PdfPCell(new Phrase("Mode of Payment"));
        c1.setHorizontalAlignment(Element.ALIGN_JUSTIFIED);
        table.addCell(c1);


        c1 = new PdfPCell(new Phrase("Order Status"));
        c1.setHorizontalAlignment(Element.ALIGN_JUSTIFIED);
        table.addCell(c1);


        c1 = new PdfPCell(new Phrase("Order Value"));
        c1.setHorizontalAlignment(Element.ALIGN_JUSTIFIED);
        table.addCell(c1);


        c1 = new PdfPCell(new Phrase("Commission Earned"));
        c1.setHorizontalAlignment(Element.ALIGN_JUSTIFIED);
        table.addCell(c1);


        table.setHeaderRows(1);
        int i = 1;
        for (OrderTransaction orderTransaction : orderTransactions) {
            table.addCell(i + " ");
            String date = DeliveryTrackUtils.convertDateToString(orderTransaction.getCreatedAt(), "dd/MM/yyyy");
            table.addCell(date);
            table.addCell(orderTransaction.getOrderId());
            table.addCell(orderTransaction.getRestName());
            table.addCell(orderTransaction.getLocation());
            table.addCell(orderTransaction.getPaymentMode());
            table.addCell("Delivered");
            table.addCell(String.valueOf(orderTransaction.getOrderAmount()));
            table.addCell(String.valueOf(orderTransaction.getCredit()));
            i++;

        }
        try {
            document.add(table);
        } catch (DocumentException e) {
            e.printStackTrace();
        }

    }


    private static void addTitlePage(Document document)
            throws DocumentException {
        Paragraph preface = new Paragraph();
        addEmptyLine(preface, 1);
        // Lets write a big header
        preface.add(new Paragraph("Montly Report", catFont));
        document.add(preface);
    }


    private static void addEmptyLine(Paragraph paragraph, int number) {
        for (int i = 0; i < number; i++) {
            paragraph.add(new Paragraph(" "));
        }
    }

}
